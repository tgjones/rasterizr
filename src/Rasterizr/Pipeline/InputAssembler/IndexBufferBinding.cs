using Rasterizr.Resources;

namespace Rasterizr.Pipeline.InputAssembler
{
	public struct IndexBufferBinding
	{
		public Buffer Buffer;
		public Format Format;
		public int Offset;

		public IndexBufferBinding(Buffer buffer, Format format, int offset)
		{
			Buffer = buffer;
			Format = format;
			Offset = offset;
		}
	}
}