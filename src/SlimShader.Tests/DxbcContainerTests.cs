using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SlimShader.IO;
using SlimShader.Tokens;

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

		[TestCase("Assets/test.bin", "Assets/test.txt")]
		public void CanParseShader(string binaryFile, string asmFile)
		{
			// Arrange.
			var binaryFileBytes = File.ReadAllBytes(binaryFile);

			// Act.
			var container = DxbcContainer.Parse(new BytecodeReader(binaryFileBytes, 0, binaryFileBytes.Length));

			// Assert.
			// Ignore first 5 lines - they contain the compiler-specific headers.
			var decompiledAsmText = string.Join(Environment.NewLine, container.ToString()
				.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
				.Skip(5));
			var asmFileText = string.Join(Environment.NewLine, File.ReadAllLines(asmFile).Skip(5));
			Assert.That(decompiledAsmText, Is.EqualTo(asmFileText));
		}
	}
}