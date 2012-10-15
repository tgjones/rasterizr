namespace SlimShader.ObjectModel
{
	public class DxbcChunkHeader
	{
		public uint FourCc { get; internal set; }
		public ChunkType ChunkType { get; internal set; }
		public uint Size { get; internal set; }
		public ChunkContent Content { get; internal set; }
	}
}