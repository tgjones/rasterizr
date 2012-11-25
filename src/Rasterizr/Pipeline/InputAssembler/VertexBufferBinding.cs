using Rasterizr.Resources;

namespace Rasterizr.Pipeline.InputAssembler
{
	public struct VertexBufferBinding
	{
		public Buffer Buffer;
		public int Offset;
		public int Stride;

		public VertexBufferBinding(Buffer buffer, int offset, int stride)
		{
			Buffer = buffer;
			Offset = offset;
			Stride = stride;
		}
	}
}