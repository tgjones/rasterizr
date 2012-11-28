using Rasterizr.Pipeline.VertexShader;

namespace Rasterizr.Pipeline.InputAssembler
{
	internal struct InputAssemblerPrimitiveOutput
	{
		public PrimitiveType PrimitiveType;
		public int PrimitiveID;
		public VertexShaderOutput[] Vertices;
	}
}