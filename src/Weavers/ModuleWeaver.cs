// This code was heavily influenced by InteropApp in SharpDX, so thanks to
// SharpDX / Alexandre Mutel for that code. Original copyright notice below.
//
// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Weavers
{
	public class ModuleWeaver
	{
        private readonly List<TypeDefinition> _classToRemoveList = new List<TypeDefinition>();
        private TypeReference _intType;
        private AssemblyDefinition _mscorlibAssembly;

        public IAssemblyResolver AssemblyResolver { get; set; }
		public ModuleDefinition ModuleDefinition { get; set; }

        public Action<string> LogInfo { get; set; }
        public Action<string> LogWarning { get; set; }
        public Action<string> LogError { get; set; }

		public void Execute()
		{
			foreach (var assemblyNameReference in ModuleDefinition.AssemblyReferences)
			{
				if (assemblyNameReference.Name.ToLower() == "mscorlib")
				{
					_mscorlibAssembly = ModuleDefinition.AssemblyResolver.Resolve(assemblyNameReference);
					break;
				}
			}

			// TODO: Temporary patch to handle correctly 4.5 Core profile
			if (_mscorlibAssembly == null)
			{
				foreach (var assemblyNameReference in ModuleDefinition.AssemblyReferences)
				{
					if (assemblyNameReference.Name == "System.Runtime")
					{
						((BaseAssemblyResolver) ModuleDefinition.AssemblyResolver).AddSearchDirectory(Path.Combine(ProgramFilesx86(), @"Reference Assemblies\Microsoft\Framework\.NETCore\v4.5"));
						_mscorlibAssembly = ModuleDefinition.AssemblyResolver.Resolve(assemblyNameReference);
						break;
					}
				}
			}

			if (_mscorlibAssembly == null)
			{
				LogError(string.Format("Missing mscorlib.dll from assembly {0}", ModuleDefinition.Assembly.FullName));
				throw new InvalidOperationException("Missing mscorlib.dll from assembly");
			}

			// Import void* and int32 from assembly using mscorlib specific version (2.0 or 4.0 depending on assembly)
            _mscorlibAssembly.MainModule.GetType("System.Void");
			_intType = ModuleDefinition.Import(_mscorlibAssembly.MainModule.GetType("System.Int32"));

			// Remove CompilationRelaxationsAttribute
			for (int i = 0; i < ModuleDefinition.Assembly.CustomAttributes.Count; i++)
			{
				var customAttribute = ModuleDefinition.Assembly.CustomAttributes[i];
				if (customAttribute.AttributeType.FullName == typeof(CompilationRelaxationsAttribute).FullName)
				{
					ModuleDefinition.Assembly.CustomAttributes.RemoveAt(i);
					i--;
				}
			}

			LogInfo(string.Format("Rasterizr interop patch for assembly [{0}]", ModuleDefinition.Assembly.FullName));
			foreach (var type in ModuleDefinition.Types)
				PatchType(type);

			// Remove All Interop classes
			foreach (var type in _classToRemoveList)
				ModuleDefinition.Types.Remove(type);

			LogInfo(string.Format("Rasterizr patch done for assembly [{0}]", ModuleDefinition.Assembly.FullName));
		}

		/// <summary>
		/// Creates the write method with the following signature: 
		/// <code>
		/// public static unsafe void* Write&lt;T&gt;(void* pDest, ref T data) where T : struct
		/// </code>
		/// </summary>
		/// <param name="method">The method to patch</param>
		private void CreateWriteMethod(MethodDefinition method)
		{
			method.Body.Instructions.Clear();
			method.Body.InitLocals = true;

			var gen = method.Body.GetILProcessor();
			var paramT = method.GenericParameters[0];
			// Preparing locals
			// local(0) int
			method.Body.Variables.Add(new VariableDefinition(_intType));
			// local(1) T*
			method.Body.Variables.Add(new VariableDefinition(new PinnedType(new ByReferenceType(paramT))));

			// Push (0) pDest for memcpy
			gen.Emit(OpCodes.Ldarg_0);

			// fixed (void* pinnedData = &data[offset])
			gen.Emit(OpCodes.Ldarg_1);
			gen.Emit(OpCodes.Stloc_1);

			// Push (1) pinnedData for memcpy
			gen.Emit(OpCodes.Ldloc_1);

			// totalSize = sizeof(T)
			gen.Emit(OpCodes.Sizeof, paramT);
			gen.Emit(OpCodes.Conv_I4);
			gen.Emit(OpCodes.Stloc_0);

			// Push (2) totalSize
			gen.Emit(OpCodes.Ldloc_0);

			// Emit cpblk
			EmitCpblk(gen);

			// Return pDest + totalSize
			gen.Emit(OpCodes.Ldloc_0);
			gen.Emit(OpCodes.Conv_I);
			gen.Emit(OpCodes.Ldarg_0);
			gen.Emit(OpCodes.Add);

			// Ret
			gen.Emit(OpCodes.Ret);
		}

		private void ReplaceSizeOfStructGeneric(ILProcessor ilProcessor, Instruction fixedtoPatch)
		{
			var paramT = ((GenericInstanceMethod) fixedtoPatch.Operand).GenericArguments[0];
			var copyInstruction = ilProcessor.Create(OpCodes.Sizeof, paramT);
			ilProcessor.Replace(fixedtoPatch, copyInstruction);
		}

		/// <summary>
		/// Creates the write range method with the following signature:
		/// <code>
		/// public static unsafe void Write&lt;T&gt;(void* pDest, T[] data, int offset, int count) where T : struct
		/// </code>
		/// </summary>
		/// <param name="method">The method copy struct.</param>
		private void CreateWriteRangeMethod(MethodDefinition method)
		{
			method.Body.Instructions.Clear();
			method.Body.InitLocals = true;

			var gen = method.Body.GetILProcessor();
			var paramT = method.GenericParameters[0];
			// Preparing locals
			// local(0) int
			method.Body.Variables.Add(new VariableDefinition(_intType));
			// local(1) T*
			method.Body.Variables.Add(new VariableDefinition(new PinnedType(new ByReferenceType(paramT))));

			// Push (0) pDest for memcpy
			gen.Emit(OpCodes.Ldarg_0);

			// fixed (void* pinnedData = &data[offset])
			gen.Emit(OpCodes.Ldarg_1);
			gen.Emit(OpCodes.Ldarg_2);
			gen.Emit(OpCodes.Ldelema, paramT);
			gen.Emit(OpCodes.Stloc_1);

			// Push (1) pinnedData for memcpy
			gen.Emit(OpCodes.Ldloc_1);

			// totalSize = sizeof(T) * count
			gen.Emit(OpCodes.Sizeof, paramT);
			gen.Emit(OpCodes.Conv_I4);
			gen.Emit(OpCodes.Ldarg_3);
			gen.Emit(OpCodes.Mul);
			gen.Emit(OpCodes.Stloc_0);

			// Push (2) totalSize
			gen.Emit(OpCodes.Ldloc_0);

			// Emit cpblk
			EmitCpblk(gen);

			// Ret
			gen.Emit(OpCodes.Ret);
		}

		/// <summary>
		/// Creates the read range method with the following signature:
		/// <code>
		/// public static unsafe void Read&lt;T&gt;(void* pSrc, T[] data, int offset, int count) where T : struct
		/// </code>
		/// </summary>
		/// <param name="method">The method copy struct.</param>
		private void CreateReadRangeMethod(MethodDefinition method)
		{
			method.Body.Instructions.Clear();
			method.Body.InitLocals = true;

			var gen = method.Body.GetILProcessor();
			var paramT = method.GenericParameters[0];
			// Preparing locals
			// local(0) int
			method.Body.Variables.Add(new VariableDefinition(_intType));
			// local(1) T*
			method.Body.Variables.Add(new VariableDefinition(new PinnedType(new ByReferenceType(paramT))));

			// fixed (void* pinnedData = &data[offset])
			gen.Emit(OpCodes.Ldarg_1);
			gen.Emit(OpCodes.Ldarg_2);
			gen.Emit(OpCodes.Ldelema, paramT);
			gen.Emit(OpCodes.Stloc_1);

			// Push (0) pinnedData for memcpy
			gen.Emit(OpCodes.Ldloc_1);

			// Push (1) pDest for memcpy
			gen.Emit(OpCodes.Ldarg_0);

			// Push (2) count
			gen.Emit(OpCodes.Ldarg_3);

			// Emit cpblk
			EmitCpblk(gen);

			// Ret
			gen.Emit(OpCodes.Ret);
		}

	    /// <summary>
	    /// Emits the cpblk method, supporting x86 and x64 platform.
	    /// </summary>
	    /// <param name="gen">The gen.</param>
	    private void EmitCpblk(ILProcessor gen)
		{
			var cpblk = gen.Create(OpCodes.Cpblk);
			//gen.Emit(OpCodes.Sizeof, voidPointerType);
			//gen.Emit(OpCodes.Ldc_I4_8);
			//gen.Emit(OpCodes.Bne_Un_S, cpblk);
			gen.Emit(OpCodes.Unaligned, (byte) 1);       // unaligned to 1
			gen.Append(cpblk);

		}

		/// <summary>
		/// Patches the method.
		/// </summary>
		/// <param name="method">The method.</param>
		private void PatchMethod(MethodDefinition method)
		{
			if (method.DeclaringType.Name == "Interop")
			{
				if (method.Name == "Read")
				{
                    CreateReadRangeMethod(method);
				}
				else if (method.Name == "Write")
				{
					if (method.Parameters.Count == 2)
						CreateWriteMethod(method);
					else
						CreateWriteRangeMethod(method);
				}
			}
			else if (method.HasBody)
			{
				var ilProcessor = method.Body.GetILProcessor();

				var instructions = method.Body.Instructions;
			    for (int i = 0; i < instructions.Count; i++)
			    {
			        var instruction = instructions[i];
			        if (instruction.OpCode == OpCodes.Call && instruction.Operand is MethodReference)
			        {
			            var methodDescription = (MethodReference) instruction.Operand;
			            if (methodDescription.DeclaringType.Name == "Interop")
			                if (methodDescription.Name.StartsWith("SizeOf"))
			                    ReplaceSizeOfStructGeneric(ilProcessor, instruction);
			        }
			    }
			}
		}

		/// <summary>
		/// Patches the type.
		/// </summary>
		/// <param name="type">The type.</param>
		private void PatchType(TypeDefinition type)
		{
			// Patch methods
			foreach (var method in type.Methods)
				PatchMethod(method);

			// Patch nested types
			foreach (var typeDefinition in type.NestedTypes)
				PatchType(typeDefinition);
		}

		/// <summary>
		/// Get Program Files x86
		/// </summary>
		/// <returns></returns>
		private static string ProgramFilesx86()
		{
			if (8 == IntPtr.Size
				|| (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
			{
				return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
			}

			return Environment.GetEnvironmentVariable("ProgramFiles");
		}
	}
}