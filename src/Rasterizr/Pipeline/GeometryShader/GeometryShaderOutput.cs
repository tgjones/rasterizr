using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.VertexShader;

namespace Rasterizr.Pipeline.GeometryShader
{
	internal struct GeometryShaderOutput
	{
		public PrimitiveType PrimitiveType;
		public VertexShaderOutput[] Vertices;
	}
}