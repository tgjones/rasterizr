using Rasterizr.Pipeline.VertexShader;

namespace Rasterizr.Pipeline.InputAssembler
{
	internal struct InputAssemblerPrimitiveOutput
	{
		public PrimitiveType PrimitiveType;
		public int PrimitiveID;

		public VertexShaderOutput Vertex0;
		public VertexShaderOutput Vertex1;
		public VertexShaderOutput Vertex2;
	}
}