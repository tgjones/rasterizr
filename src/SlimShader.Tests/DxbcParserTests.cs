using System.IO;
using NUnit.Framework;
using SlimShader.ObjectModel;

namespace SlimShader.Tests
{
	/// <summary>
	/// Using documentation from d3d10tokenizedprogramformat.hpp.
	/// </summary>
	[TestFixture]
	public class DxbcParserTests
	{
		[Test]
		public void CanLoadShaderBytecode()
		{
			// Arrange.
			var parser = new DxbcParser();
			
			// Act.
			var container = parser.Parse(new DxbcReader(File.OpenRead("Assets/test.bin")));

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
			Assert.That(shaderProgram.Version.ShaderType, Is.EqualTo(ShaderType.PixelShader));
			Assert.That(shaderProgram.Length, Is.EqualTo(1241));
			
		}
	}
}