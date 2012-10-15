using System.Collections.Generic;
using System.IO;
using SlimShader.ObjectModel;

namespace SlimShader
{
	public class DxbcParser
	{
		private static readonly Dictionary<uint, ChunkType> KnownChunkTypes = new Dictionary<uint, ChunkType>
		{
			{ DxbcReader.GetFourCc('S', 'H', 'D', 'R'), ChunkType.Shdr },
			{ DxbcReader.GetFourCc('S', 'H', 'E', 'X'), ChunkType.Shex }
		};

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
				if (KnownChunkTypes.ContainsKey(chunk.FourCc))
					chunk.ChunkType = KnownChunkTypes[chunk.FourCc];

				switch (chunk.ChunkType)
				{
					case ChunkType.Shdr:
					case ChunkType.Shex:
						chunk.Content = new ShaderProgramParser().Parse(reader);
						break;
				}

				container.ChunkMap[chunk.FourCc] = chunk;
				container.Chunks.Add(chunk);
			}

			return container;
		}
	}
}