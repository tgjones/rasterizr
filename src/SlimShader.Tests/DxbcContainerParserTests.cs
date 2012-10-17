using System.IO;
using NUnit.Framework;
using SlimShader.ObjectModel;
using SlimShader.ObjectModel.Tokens;
using SlimShader.Parser;

namespace SlimShader.Tests
{
	/// <summary>
	/// Using documentation from d3d10tokenizedprogramformat.hpp.
	/// </summary>
	[TestFixture]
	public class DxbcContainerParserTests
	{
		[Test]
		public void CanLoadShaderBytecode()
		{
			// Arrange.
			var binaryReader = new BinaryReader(File.OpenRead("Assets/test.bin"));
			var parser = new DxbcContainerParser(new DxbcReader(binaryReader));
			
			// Act.
			var container = parser.Parse();

			// Assert.
			Assert.That(container.Header.FourCc, Is.EqualTo(1128421444));
			Assert.That(container.Header.UniqueKey[0], Is.EqualTo(2210296095));
			Assert.That(container.Header.UniqueKey[1], Is.EqualTo(678178285));
			Assert.That(container.Header.UniqueKey[2], Is.EqualTo(4191542541));
			Assert.That(container.Header.UniqueKey[3], Is.EqualTo(1829059345));
			Assert.That(container.Header.One, Is.EqualTo(1));
			Assert.That(container.Header.TotalSize, Is.EqualTo(5864));
			Assert.That(container.Header.ChunkCount, Is.EqualTo(5));

			Assert.That(container.ChunkMap.Count, Is.EqualTo(5));
			Assert.That(container.Chunks.Count, Is.EqualTo(5));
			Assert.That(container.Chunks[0].FourCc, Is.EqualTo(1178944594));
			Assert.That(container.Chunks[0].Size, Is.EqualTo(544));
			Assert.That(container.Chunks[1].FourCc, Is.EqualTo(1313297225));
			Assert.That(container.Chunks[1].Size, Is.EqualTo(104));
			Assert.That(container.Chunks[2].FourCc, Is.EqualTo(1313297231));
			Assert.That(container.Chunks[2].Size, Is.EqualTo(44));
			Assert.That(container.Chunks[3].FourCc, Is.EqualTo(1380206675));
			Assert.That(container.Chunks[3].Size, Is.EqualTo(4964));
			Assert.That(container.Chunks[3].ChunkType, Is.EqualTo(ChunkType.Shdr));
			Assert.That(container.Chunks[4].FourCc, Is.EqualTo(1413567571));
			Assert.That(container.Chunks[4].Size, Is.EqualTo(116));

			var shaderProgram = (ShaderProgram) container.Chunks[3].Content;
			Assert.That(shaderProgram.Version.MajorVersion, Is.EqualTo(4));
			Assert.That(shaderProgram.Version.MinorVersion, Is.EqualTo(0));
			Assert.That(shaderProgram.Version.ProgramType, Is.EqualTo(ProgramType.PixelShader));
			Assert.That(shaderProgram.Length, Is.EqualTo(1241));

			Assert.That(shaderProgram.Tokens, Has.Count.EqualTo(8));

			Assert.That(shaderProgram.Tokens[2], Is.InstanceOf<DeclareResourceToken>());
			var resourceToken1 = (DeclareResourceToken) shaderProgram.Tokens[2];
			Assert.That(resourceToken1.Header.IsExtended, Is.False);
			Assert.That(resourceToken1.Header.Length, Is.EqualTo(4));
			Assert.That(resourceToken1.Header.OpcodeType, Is.EqualTo(OpcodeType.SM4_OPCODE_DCL_RESOURCE));
			Assert.That(resourceToken1.ResourceDimension, Is.EqualTo(ResourceDimension.RESOURCE_DIMENSION_TEXTURE2D));
			Assert.That(resourceToken1.ReturnType.X, Is.EqualTo(ResourceReturnType.D3D10_SB_RETURN_TYPE_FLOAT));
			Assert.That(resourceToken1.ReturnType.Y, Is.EqualTo(ResourceReturnType.D3D10_SB_RETURN_TYPE_FLOAT));
			Assert.That(resourceToken1.ReturnType.Z, Is.EqualTo(ResourceReturnType.D3D10_SB_RETURN_TYPE_FLOAT));
			Assert.That(resourceToken1.ReturnType.W, Is.EqualTo(ResourceReturnType.D3D10_SB_RETURN_TYPE_FLOAT));
			Assert.That(resourceToken1.SampleCount, Is.EqualTo(0));
			Assert.That(resourceToken1.Operand.OperandType, Is.EqualTo(OperandType.OPERAND_TYPE_RESOURCE));
			Assert.That(resourceToken1.Operand.Modifier, Is.EqualTo(OperandModifier.OPERAND_MODIFIER_NONE));
			Assert.That(resourceToken1.Operand.IsExtended, Is.False);
			Assert.That(resourceToken1.Operand.ComponentMask, Is.EqualTo(0));
			Assert.That(resourceToken1.Operand.IndexDimension, Is.EqualTo(OperandIndexDimension.D3D10_SB_OPERAND_INDEX_1D));
			Assert.That(resourceToken1.Operand.IndexRepresentations[0], Is.EqualTo(OperandIndexRepresentation.OPERAND_INDEX_IMMEDIATE32));
			Assert.That(resourceToken1.Operand.ArraySizes[0], Is.EqualTo(0));
		}
	}
}