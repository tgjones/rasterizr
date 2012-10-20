namespace SlimShader.ObjectModel
{
	public class DxbcChunk
	{
		public uint FourCc { get; internal set; }
		public ChunkType ChunkType { get; internal set; }
		public uint Size { get; internal set; }
		public ChunkContent Content { get; internal set; }

		public override string ToString()
		{
			if (Content != null)
				return Content.ToString();
			return base.ToString();
		}
	}
}