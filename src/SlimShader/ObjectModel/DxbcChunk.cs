using System;
using System.Collections.Generic;
using SlimShader.IO;
using SlimShader.Parser;

namespace SlimShader.ObjectModel
{
	public abstract class DxbcChunk
	{
		private static readonly Dictionary<uint, ChunkType> KnownChunkTypes = new Dictionary<uint, ChunkType>
		{
			{ "ISGN".ToFourCc(), ChunkType.Isgn },
			{ "OSGN".ToFourCc(), ChunkType.Osgn },
			{ "RDEF".ToFourCc(), ChunkType.Rdef },
			{ "SHDR".ToFourCc(), ChunkType.Shdr },
			{ "SHEX".ToFourCc(), ChunkType.Shex },
			{ "STAT".ToFourCc(), ChunkType.Stat }
		};

		public uint FourCc { get; internal set; }
		public ChunkType ChunkType { get; internal set; }
		public uint ChunkSize { get; internal set; }

		public static DxbcChunk ParseChunk(BytecodeReader chunkReader)
		{
			// Type of chunk this is.
			uint fourCc = chunkReader.ReadUInt32();

			// Total length of the chunk in bytes.
			uint chunkSize = chunkReader.ReadUInt32();

			ChunkType chunkType;
			if (KnownChunkTypes.ContainsKey(fourCc))
				chunkType = KnownChunkTypes[fourCc];
			else
				throw new NotSupportedException("Chunk type '" + fourCc.ToFourCcString() + "' is not yet supported.");

			var chunkContentReader = chunkReader.CopyAtCurrentPosition((int) chunkSize);
			DxbcChunk chunk;
			switch (chunkType)
			{
				case ChunkType.Isgn :
					chunk = InputSignatureChunk.Parse(chunkContentReader);
					break;
				case ChunkType.Osgn:
					chunk = OutputSignatureChunk.Parse(chunkContentReader);
					break;
				case ChunkType.Rdef:
					chunk = ResourceDefinitionChunk.Parse(chunkContentReader);
					break;
				case ChunkType.Shdr:
				case ChunkType.Shex:
					chunk = ShaderProgramChunk.Parse(chunkContentReader);
					break;
				case ChunkType.Stat:
					chunk = StatChunk.Parse(chunkContentReader);
					break;
				default :
					throw new ArgumentOutOfRangeException();
			}

			chunk.FourCc = fourCc;
			chunk.ChunkSize = chunkSize;
			chunk.ChunkType = chunkType;

			return chunk;
		}
	}
}