using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SharpDX.D3DCompiler;
using SlimShader.IO;
using SlimShader.ResourceDefinition;
using SlimShader.Shader;
using SlimShader.Shader.Tokens;
using ResourceDimension = SlimShader.Shader.ResourceDimension;
using ResourceReturnType = SlimShader.Shader.ResourceReturnType;

namespace SlimShader.Tests
{
	/// <summary>
	/// Using documentation from d3d11tokenizedprogramformat.hpp.
	/// </summary>
	[TestFixture]
	public class DxbcContainerTests
	{
		[Test]
		public void CanLoadShaderBytecode()
		{
			// Arrange.
			var fileBytes = File.ReadAllBytes("Assets/test.bin");
			
			// Act.
			var container = DxbcContainer.Parse(new BytecodeReader(fileBytes, 0, fileBytes.Length));

			// Assert.
			Assert.That(container.Header.FourCc, Is.EqualTo(1128421444));
			Assert.That(container.Header.UniqueKey[0], Is.EqualTo(2210296095));
			Assert.That(container.Header.UniqueKey[1], Is.EqualTo(678178285));
			Assert.That(container.Header.UniqueKey[2], Is.EqualTo(4191542541));
			Assert.That(container.Header.UniqueKey[3], Is.EqualTo(1829059345));
			Assert.That(container.Header.One, Is.EqualTo(1));
			Assert.That(container.Header.TotalSize, Is.EqualTo(5864));
			Assert.That(container.Header.ChunkCount, Is.EqualTo(5));

			Assert.That(container.Chunks.Count, Is.EqualTo(5));
			Assert.That(container.Chunks[0].FourCc, Is.EqualTo(1178944594));
			Assert.That(container.Chunks[0].ChunkSize, Is.EqualTo(544));
			Assert.That(container.Chunks[1].FourCc, Is.EqualTo(1313297225));
			Assert.That(container.Chunks[1].ChunkSize, Is.EqualTo(104));
			Assert.That(container.Chunks[2].FourCc, Is.EqualTo(1313297231));
			Assert.That(container.Chunks[2].ChunkSize, Is.EqualTo(44));
			Assert.That(container.Chunks[3].FourCc, Is.EqualTo(1380206675));
			Assert.That(container.Chunks[3].ChunkSize, Is.EqualTo(4964));
			Assert.That(container.Chunks[3].ChunkType, Is.EqualTo(ChunkType.Shdr));
			Assert.That(container.Chunks[4].FourCc, Is.EqualTo(1413567571));
			Assert.That(container.Chunks[4].ChunkSize, Is.EqualTo(116));

			var shaderProgram = (ShaderProgramChunk) container.Chunks[3];
			Assert.That(shaderProgram.Version.MajorVersion, Is.EqualTo(4));
			Assert.That(shaderProgram.Version.MinorVersion, Is.EqualTo(0));
			Assert.That(shaderProgram.Version.ProgramType, Is.EqualTo(ProgramType.PixelShader));
			Assert.That(shaderProgram.Length, Is.EqualTo(1241));

			Assert.That(shaderProgram.Tokens, Has.Count.EqualTo(213));

			Assert.That(shaderProgram.Tokens[3], Is.InstanceOf<ResourceDeclarationToken>());
			var resourceToken1 = (ResourceDeclarationToken) shaderProgram.Tokens[3];
			Assert.That(resourceToken1.Header.IsExtended, Is.False);
			Assert.That(resourceToken1.Header.Length, Is.EqualTo(4));
			Assert.That(resourceToken1.Header.OpcodeType, Is.EqualTo(OpcodeType.DclResource));
			Assert.That(resourceToken1.ResourceDimension, Is.EqualTo(ResourceDimension.Texture2D));
			Assert.That(resourceToken1.ReturnType.X, Is.EqualTo(ResourceReturnType.Float));
			Assert.That(resourceToken1.ReturnType.Y, Is.EqualTo(ResourceReturnType.Float));
			Assert.That(resourceToken1.ReturnType.Z, Is.EqualTo(ResourceReturnType.Float));
			Assert.That(resourceToken1.ReturnType.W, Is.EqualTo(ResourceReturnType.Float));
			Assert.That(resourceToken1.SampleCount, Is.EqualTo(0));
			Assert.That(resourceToken1.Operand.OperandType, Is.EqualTo(OperandType.Resource));
			Assert.That(resourceToken1.Operand.Modifier, Is.EqualTo(OperandModifier.None));
			Assert.That(resourceToken1.Operand.IsExtended, Is.False);
			Assert.That(resourceToken1.Operand.ComponentMask, Is.EqualTo(ComponentMask.None));
			Assert.That(resourceToken1.Operand.IndexDimension, Is.EqualTo(OperandIndexDimension._1D));
			Assert.That(resourceToken1.Operand.IndexRepresentations[0], Is.EqualTo(OperandIndexRepresentation.Immediate32));
			Assert.That(resourceToken1.Operand.Indices[0].Value, Is.EqualTo(0));
		}

		[TestCase("Shaders/test")]
		[TestCase("Shaders/ps4/fxaa")]
		[TestCase("Shaders/ps4/primID")]
		[TestCase("Shaders/ps5/conservative_depth_ge")]
		[TestCase("Shaders/ps5/interfaces")]
		[TestCase("Shaders/ps5/sample")]
		[TestCase("Shaders/vs4/mov")]
		[TestCase("Shaders/vs4/multiple_const_buffers")]
		[TestCase("Shaders/vs4/switch")]
		[TestCase("Shaders/vs5/any")]
		[TestCase("Shaders/vs5/const_temp")]
		[TestCase("Shaders/vs5/mad_imm")]
		[TestCase("Shaders/vs5/mov")]
		[TestCase("Shaders/vs5/sincos")]
		[TestCase("Shaders/Sdk/DynamicShaderLinkage11_PS")]
		public void CanParseShader(string file)
		{
			// Arrange.
			string binaryFile = file + ".o";
			string asmFile = file + ".asm";
			var asmFileText = GetAsmText(asmFile);
			var binaryFileBytes = File.ReadAllBytes(binaryFile);

			// Act.
			var container = DxbcContainer.Parse(new BytecodeReader(binaryFileBytes, 0, binaryFileBytes.Length));

			// Assert.
			// Ignore first 5 lines - they contain the compiler-specific headers.
			var decompiledAsmText = string.Join(Environment.NewLine, container.ToString()
				.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
				.Skip(5).Select(x => x.Trim()));
			Assert.That(decompiledAsmText, Is.EqualTo(asmFileText));

			// Compare with actual Direct3D reflected objects.
			var shaderBytecode = ShaderBytecode.FromFile(binaryFile);
			var shaderReflection = new ShaderReflection(shaderBytecode);
			AssertAreEqual(shaderReflection.Description, container.ResourceDefinition);
			AssertAreEqual(shaderReflection.Description, container.Statistics);
			AssertAreEqual(shaderReflection, container.Statistics);
			Assert.AreEqual(shaderReflection.Description.InputParameters, container.InputSignature.Parameters.Count);
			Assert.AreEqual(shaderReflection.Description.OutputParameters, container.OutputSignature.Parameters.Count);
		}

		private static string GetAsmText(string asmFile)
		{
			var asmFileLines = File.ReadAllLines(asmFile);

			/* The first 5 or 6 lines contain something like:
			
			//
			// Generated by Microsoft (R) HLSL Shader Compiler 9.29.952.3111
			//
			//
			//   fxc /T vs_4_0 /Fo multiple_const_buffers.o /Fc multiple_const_buffers.asm
			//    multiple_const_buffers
			*/

			// We want to skip all that, because we can't accurately recreate the fxc command-line, and so we
			// aren't able to do a string comparison on these lines.
			int skip = 5;
			while (asmFileLines[skip] != "//")
				skip++;
			return string.Join(Environment.NewLine, asmFileLines.Skip(skip).Select(x => x.Trim()));
		}

		private static void AssertAreEqual(ShaderDescription expected, StatisticsChunk actual)
		{
			Assert.AreEqual(expected.ArrayInstructionCount, actual.ArrayInstructionCount);
			Assert.AreEqual(expected.BarrierInstructions, actual.BarrierInstructions);
			Assert.AreEqual(expected.ControlPoints, actual.ControlPoints);
			Assert.AreEqual(expected.CutInstructionCount, actual.CutInstructionCount);
			Assert.AreEqual(expected.DeclarationCount, actual.DeclarationCount);
			Assert.AreEqual(expected.DefineCount, actual.DefineCount);
			Assert.AreEqual(expected.DynamicFlowControlCount, actual.DynamicFlowControlCount);
			Assert.AreEqual(expected.EmitInstructionCount, actual.EmitInstructionCount);
			Assert.AreEqual(expected.FloatInstructionCount, actual.FloatInstructionCount);
			Assert.AreEqual(expected.GeometryShaderInstanceCount, actual.GeometryShaderInstanceCount);
			Assert.AreEqual(expected.GeometryShaderMaxOutputVertexCount, actual.GeometryShaderMaxOutputVertexCount);
			Assert.AreEqual((int) expected.GeometryShaderOutputTopology, (int) actual.GeometryShaderOutputTopology);
			Assert.AreEqual((int) expected.HullShaderOutputPrimitive, (int) actual.HullShaderOutputPrimitive);
			Assert.AreEqual((int) expected.HullShaderPartitioning, (int) actual.HullShaderPartitioning);
			Assert.AreEqual((int) expected.InputPrimitive, (int) actual.InputPrimitive);
			Assert.AreEqual(expected.InstructionCount, actual.InstructionCount);
			Assert.AreEqual(expected.InterlockedInstructions, actual.InterlockedInstructions);
			Assert.AreEqual(expected.IntInstructionCount, actual.IntInstructionCount);
			Assert.AreEqual(expected.MacroInstructionCount, actual.MacroInstructionCount);
			Assert.AreEqual(expected.PatchConstantParameters, actual.PatchConstantParameters);
			Assert.AreEqual(expected.StaticFlowControlCount, actual.StaticFlowControlCount);
			Assert.AreEqual(expected.TempArrayCount, actual.TempArrayCount);
			Assert.AreEqual(expected.TempRegisterCount, actual.TempRegisterCount);
			Assert.AreEqual((int) expected.TessellatorDomain, (int) actual.TessellatorDomain);
			Assert.AreEqual(expected.TextureBiasInstructions, actual.TextureBiasInstructions);
			Assert.AreEqual(expected.TextureCompInstructions, actual.TextureCompInstructions);
			Assert.AreEqual(expected.TextureGradientInstructions, actual.TextureGradientInstructions);
			Assert.AreEqual(expected.TextureLoadInstructions, actual.TextureLoadInstructions);
			Assert.AreEqual(expected.TextureNormalInstructions, actual.TextureNormalInstructions);
			Assert.AreEqual(expected.TextureStoreInstructions, actual.TextureStoreInstructions);
			Assert.AreEqual(expected.UintInstructionCount, actual.UIntInstructionCount);
		}

		private static void AssertAreEqual(ShaderReflection expected, StatisticsChunk actual)
		{
			Assert.AreEqual(expected.ConditionalMoveInstructionCount, actual.MovCInstructionCount);
			Assert.AreEqual(expected.ConversionInstructionCount, actual.ConversionInstructionCount);
			Assert.AreEqual(expected.MoveInstructionCount, actual.MovInstructionCount);
		}

		private static void AssertAreEqual(ShaderDescription expected, ResourceDefinitionChunk actual)
		{
			Assert.AreEqual(expected.Creator, actual.Creator);
			Assert.AreEqual((int) expected.Flags, (int) actual.Flags);
			//Assert.AreEqual(expected.Version, actual.Target);
			Assert.AreEqual(expected.BoundResources, actual.ResourceBindings.Count);
			Assert.AreEqual(expected.ConstantBuffers, actual.ConstantBuffers.Count);
		}
	}
}