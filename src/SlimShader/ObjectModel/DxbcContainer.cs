using System.Collections.Generic;

namespace SlimShader.ObjectModel
{
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
}