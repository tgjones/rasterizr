using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace SlimShader.Tests
{
	/// <summary>
	/// Using documentation from d3d10tokenizedprogramformat.hpp.
	/// </summary>
	[TestFixture]
	public class ShaderLoaderTests
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
			Assert.That(container.Chunks[4].FourCc, Is.EqualTo(1413567571));
			Assert.That(container.Chunks[4].Size, Is.EqualTo(116));
		}
	}

	public class Shader
	{
		public uint MajorVersion { get; set; }
		public uint MinorVersion { get; set; }
		public ShaderType ShaderType { get; set; }
		public uint Length { get; set; }
	}

	public class DxbcParser
	{
		public DxbcContainer Parse(DxbcReader reader)
		{
			var container = new DxbcContainer();
			container.Header = reader.ReadContainerHeader();

			var chunkCount = container.Header.ChunkCount;
			var chunkOffsets = new uint[chunkCount];
			for (uint i = 0; i < chunkCount; i++)
				chunkOffsets[i] = reader.ReadUInt32();

			for (uint i = 0; i < chunkCount; i++)
			{
				uint offset = chunkOffsets[i];
				reader.BaseStream.Seek(offset, SeekOrigin.Begin);
				var chunk = reader.ReadChunkHeader();

				container.ChunkMap[chunk.FourCc] = chunk;
				container.Chunks.Add(chunk);
			}

			return container;
		}
	}

	public class ParseException : ApplicationException
	{
		public ParseException(string message)
			 : base(message)
		{
			
		}
	}

	public class DxbcReader : BinaryReader
	{
		public DxbcReader(Stream input)
			: base(input)
		{
		}

		private static uint GetFourCc(char a, char b, char c, char d)
		{
			return a | ((uint) (b << 8)) | ((uint) c << 16) | ((uint) d << 24);
		}

		public DxbcContainerHeader ReadContainerHeader()
		{
			uint fourCcDxbc = GetFourCc('D', 'X', 'B', 'C');
			uint fourCc = ReadUInt32();
			if (fourCc != fourCcDxbc)
				throw new ParseException("Invalid FourCC");

			var uniqueKey = new uint[4];
			uniqueKey[0] = ReadUInt32();
			uniqueKey[1] = ReadUInt32();
			uniqueKey[2] = ReadUInt32();
			uniqueKey[3] = ReadUInt32();

			return new DxbcContainerHeader
			{
				FourCc = fourCc,
				UniqueKey = uniqueKey,
				One = ReadUInt32(),
				TotalSize = ReadUInt32(),
				ChunkCount = ReadUInt32()
			};
		}

		public DxbcChunkHeader ReadChunkHeader()
		{
			return new DxbcChunkHeader
			{
				FourCc = ReadUInt32(),
				Size = ReadUInt32()
			};
		}
	}

	public class ShaderLoader
	{
		private readonly BinaryReader _reader;
		private readonly Shader _shader;

		public Shader Shader
		{
			get { return _shader; }
		}

		public ShaderLoader(BinaryReader reader)
		{
			_reader = reader;
			_shader = new Shader();

			ReadVersionToken();
		}

		private void ReadVersionToken()
		{
			var versionToken = _reader.ReadUInt32();
			_shader.MajorVersion = DecodeProgramMajorVersion(versionToken);
			_shader.MinorVersion = DecodeProgramMinorVersion(versionToken);
			_shader.ShaderType = DecodeShaderType(versionToken);
			_shader.Length = _reader.ReadUInt32();
		}

		private static ShaderType DecodeShaderType(uint ui32Token)
		{
			return (ShaderType)((ui32Token & 0xffff0000) >> 16);
		}

		private static uint DecodeProgramMajorVersion(uint ui32Token)
		{
			return (ui32Token & 0x000000f0) >> 4;
		}

		private static uint DecodeProgramMinorVersion(uint ui32Token)
		{
			return (ui32Token & 0x0000000f);
		}

		private static uint DecodeInstructionLength(uint ui32Token)
		{
			return (ui32Token & 0x7f000000) >> 24;
		}
	}

	public enum ShaderType
	{
		PixelShader = 0,
		VertexShader = 1,
		GeometryShader = 2
	}

	public class DxbcContainer
	{
		public DxbcContainerHeader Header { get; internal set; }
		public List<DxbcChunkHeader> Chunks { get; private set; }
		public Dictionary<uint, DxbcChunkHeader> ChunkMap { get; private set; }

		public DxbcContainer()
		{
			Chunks = new List<DxbcChunkHeader>();
			ChunkMap = new Dictionary<uint, DxbcChunkHeader>();
		}
	}

	public class DxbcChunkHeader
	{
		public uint FourCc { get; set; }
		public uint Size { get; set; }
	}

	public class DxbcContainerHeader
	{
		public uint FourCc { get; internal set; }
		public uint[] UniqueKey { get; internal set; }
		public uint One { get; internal set; }
		public uint TotalSize { get; internal set; }
		public uint ChunkCount { get; internal set; }
	}
}