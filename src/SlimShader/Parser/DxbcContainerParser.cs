using System.Collections.Generic;
using System.IO;
using SlimShader.ObjectModel;

namespace SlimShader.Parser
{
	public class DxbcContainerParser : DxbcParser
	{
		private static readonly Dictionary<uint, ChunkType> KnownChunkTypes = new Dictionary<uint, ChunkType>
		{
			{ GetFourCc('S', 'H', 'D', 'R'), ChunkType.Shdr },
			{ GetFourCc('S', 'H', 'E', 'X'), ChunkType.Shex }
		};

		private readonly DxbcReader _reader;

		public DxbcContainerParser(DxbcReader reader)
		{
			_reader = reader;
		}

		public DxbcContainer Parse()
		{
			var container = new DxbcContainer();
			container.Header = ReadContainerHeader();

			var chunkCount = container.Header.ChunkCount;
			var chunkOffsets = new uint[chunkCount];
			for (uint i = 0; i < chunkCount; i++)
				chunkOffsets[i] = _reader.ReadAndMoveNext();

			for (uint i = 0; i < chunkCount; i++)
			{
				uint offset = chunkOffsets[i];
				_reader.Seek(offset, SeekOrigin.Begin);
				var chunk = ReadChunkHeader();
				if (KnownChunkTypes.ContainsKey(chunk.FourCc))
					chunk.ChunkType = KnownChunkTypes[chunk.FourCc];

				switch (chunk.ChunkType)
				{
					case ChunkType.Shdr:
					case ChunkType.Shex:
						chunk.Content = new ShaderProgramParser(_reader).Parse();
						break;
				}

				container.ChunkMap[chunk.FourCc] = chunk;
				container.Chunks.Add(chunk);
			}

			return container;
		}

		private DxbcContainerHeader ReadContainerHeader()
		{
			uint fourCcDxbc = GetFourCc('D', 'X', 'B', 'C');
			uint fourCc = _reader.ReadAndMoveNext();
			if (fourCc != fourCcDxbc)
				throw new ParseException("Invalid FourCC");

			var uniqueKey = new uint[4];
			uniqueKey[0] = _reader.ReadAndMoveNext();
			uniqueKey[1] = _reader.ReadAndMoveNext();
			uniqueKey[2] = _reader.ReadAndMoveNext();
			uniqueKey[3] = _reader.ReadAndMoveNext();

			return new DxbcContainerHeader
			{
				FourCc = fourCc,
				UniqueKey = uniqueKey,
				One = _reader.ReadAndMoveNext(),
				TotalSize = _reader.ReadAndMoveNext(),
				ChunkCount = _reader.ReadAndMoveNext()
			};
		}

		private DxbcChunkHeader ReadChunkHeader()
		{
			return new DxbcChunkHeader
			{
				FourCc = _reader.ReadAndMoveNext(),
				Size = _reader.ReadAndMoveNext()
			};
		}
	}
}