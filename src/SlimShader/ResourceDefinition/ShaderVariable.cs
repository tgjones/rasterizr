using System.Collections.Generic;
using SlimShader.IO;
using SlimShader.Shader;

namespace SlimShader.ResourceDefinition
{
	/// <summary>
	/// Describes a shader variable.
	/// Based on D3D11_SHADER_VARIABLE_DESC.
	/// </summary>
	public class ShaderVariable
	{
		/// <summary>
		/// The variable name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the name of the base class.
		/// </summary>
		public string BaseType { get; private set; }

		/// <summary>
		/// Offset from the start of the parent structure, to the beginning of the variable.
		/// </summary>
		public uint StartOffset { get; private set; }

		/// <summary>
		/// Size of the variable (in bytes).
		/// </summary>
		public uint Size { get; set; }

		/// <summary>
		/// Flags, which identify shader-variable properties.
		/// </summary>
		public ShaderVariableFlags Flags { get; private set; }

		/// <summary>
		/// Get a shader-variable type.
		/// </summary>
		public ShaderType ShaderType { get; private set; }

		/// <summary>
		/// The default value for initializing the variable.
		/// </summary>
		public object DefaultValue { get; private set; }

		/// <summary>
		/// Gets the corresponding interface slot for a variable that represents an interface pointer.
		/// </summary>
		public List<uint> InterfaceSlots { get; private set; }

		public static ShaderVariable Parse(BytecodeReader reader, BytecodeReader variableReader, ShaderVersion target)
		{
			uint nameOffset = variableReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int) nameOffset);

			var startOffset = variableReader.ReadUInt32();
			uint size = variableReader.ReadUInt32();
			var flags = (ShaderVariableFlags) variableReader.ReadUInt32();

			var typeOffset = variableReader.ReadUInt32();
			var typeReader = reader.CopyAtOffset((int) typeOffset);
			var shaderType = ShaderType.Parse(reader, typeReader, target);

			var defaultValueOffset = variableReader.ReadUInt32();
			if (defaultValueOffset != 0)
			{
				var defaultValueReader = reader.CopyAtOffset((int) defaultValueOffset);
				// TODO: Read default value
				// https://github.com/mirrors/wine/blob/master/dlls/d3dcompiler_43/reflection.c#L1362
			}

			if (target.MajorVersion >= 5)
			{
				// https://github.com/mirrors/wine/blob/master/dlls/d3dcompiler_43/reflection.c#L1371
				// TODO: Work out what these unknown values are.
				uint unknown1 = variableReader.ReadUInt32();
				uint unknown2 = variableReader.ReadUInt32();
				uint unknown3 = variableReader.ReadUInt32();
				uint unknown4 = variableReader.ReadUInt32();
			}

			return new ShaderVariable
			{
				Name = nameReader.ReadString(),
				BaseType = nameReader.ReadString(),
				StartOffset = startOffset,
				Size = size,
				Flags = flags,
				ShaderType = shaderType
			};
		}

		public override string ToString()
		{
			// For example:
			// row_major modelview;               // Offset:    0 Size:    64
			// float4x4 modelview;                // Offset:    0 Size:    64
			// int unusedTestA;                   // Offset:   64 Size:     4 [unused]
			// float4 cool;                       // Offset:    0 Size:    16

			string variableType = string.Empty;
			switch (ShaderType.VariableClass)
			{
				case ShaderVariableClass.MatrixRows :
					variableType += ShaderType.VariableClass.GetDescription() + " ";
					break;
			}
			variableType += ShaderType.VariableType.GetDescription();
			if (ShaderType.Columns > 1 && ShaderType.VariableType != ShaderVariableType.InterfacePointer)
			{
				variableType += ShaderType.Columns;
				if (ShaderType.Rows > 1)
					variableType += "x" + ShaderType.Rows;
			}

			string arrayCount = string.Empty;
			if (ShaderType.ElementCount > 0)
				arrayCount = "[" + ShaderType.ElementCount + "]";

			if (!string.IsNullOrEmpty(ShaderType.BaseTypeName))
				variableType += " " + ShaderType.BaseTypeName;

			string declaration = string.Format("{0} {1}{2};", variableType, Name, arrayCount);
			string result = string.Format("{0,-35}// Offset: {1,4} Size: {2,5}", declaration, StartOffset, Size);

			if (!Flags.HasFlag(ShaderVariableFlags.Used))
				result += " [unused]";

			return result;
		}
	}
}