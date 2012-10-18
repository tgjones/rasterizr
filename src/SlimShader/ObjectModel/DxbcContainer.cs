using System.Collections.Generic;

namespace SlimShader.ObjectModel
{
	public class DxbcContainer
	{
		public DxbcContainerHeader Header { get; internal set; }
		public List<DxbcChunk> Chunks { get; private set; }

		public DxbcContainer()
		{
			Chunks = new List<DxbcChunk>();
		}
	}
}