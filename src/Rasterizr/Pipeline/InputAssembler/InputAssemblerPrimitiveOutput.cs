using Rasterizr.Pipeline.VertexShader;

namespace Rasterizr.Pipeline.InputAssembler
{
	internal struct InputAssemblerPrimitiveOutput
	{
		public int PrimitiveID;
	    public uint RenderTargetArrayIndex;
		public VertexShaderOutput[] Vertices;
	}
}