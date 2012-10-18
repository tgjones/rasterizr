using System.Collections.Generic;
using SlimShader.IO;
using SlimShader.ObjectModel;

namespace SlimShader.Parser
{
	public class DxbcContainerParser : BytecodeParser<DxbcContainer>
	{
		private static readonly Dictionary<uint, ChunkType> KnownChunkTypes = new Dictionary<uint, ChunkType>
		{
			{ "SHDR".ToFourCc(), ChunkType.Shdr },
			{ "SHEX".ToFourCc(), ChunkType.Shex },
			{ "RDEF".ToFourCc(), ChunkType.Rdef }
		};

		public DxbcContainerParser(BytecodeReader reader)
			: base(reader)
		{
			
		}

		public override DxbcContainer Parse()
		{
			var container = new DxbcContainer();

			uint fourCc = Reader.ReadUInt32();
			if (fourCc != "DXBC".ToFourCc())
				throw new ParseException("Invalid FourCC");

			var uniqueKey = new uint[4];
			uniqueKey[0] = Reader.ReadUInt32();
			uniqueKey[1] = Reader.ReadUInt32();
			uniqueKey[2] = Reader.ReadUInt32();
			uniqueKey[3] = Reader.ReadUInt32();

			container.Header = new DxbcContainerHeader
			{
				FourCc = fourCc,
				UniqueKey = uniqueKey,
				One = Reader.ReadUInt32(),
				TotalSize = Reader.ReadUInt32(),
				ChunkCount = Reader.ReadUInt32()
			};

			for (uint i = 0; i < container.Header.ChunkCount; i++)
			{
				uint chunkOffset = Reader.ReadUInt32();
				var chunkReader = Reader.CopyAtOffset((int) chunkOffset);
				container.Chunks.Add(ParseChunk(chunkReader));
			}

			return container;
		}

		private DxbcChunk ParseChunk(BytecodeReader chunkReader)
		{
			var chunk = new DxbcChunk();

			// Type of chunk this is.
			chunk.FourCc = chunkReader.ReadUInt32();

			// Total length of the chunk in bytes.
			chunk.Size = chunkReader.ReadUInt32();

			if (KnownChunkTypes.ContainsKey(chunk.FourCc))
				chunk.ChunkType = KnownChunkTypes[chunk.FourCc];

			var chunkContentReader = chunkReader.CopyAtCurrentPosition((int) chunk.Size);
			switch (chunk.ChunkType)
			{
				case ChunkType.Shdr:
				case ChunkType.Shex:
					chunk.Content = new ShaderProgramParser(chunkContentReader).Parse();
					break;
				//case ChunkType.Rdef :
				//	chunk.Content = new ResourceDefinitionParser(_reader).Parse();
				//	break;
			}

			return chunk;
		}
	}
}