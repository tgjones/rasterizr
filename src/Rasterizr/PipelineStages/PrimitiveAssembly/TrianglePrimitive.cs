using System.Collections.Generic;
using Rasterizr.PipelineStages.ShaderStages.VertexShader;

namespace Rasterizr.PipelineStages.PrimitiveAssembly
{
	public struct TrianglePrimitive
	{
		public VertexShaderOutput V1;
		public VertexShaderOutput V2;
		public VertexShaderOutput V3;

		public TrianglePrimitive(VertexShaderOutput v1, VertexShaderOutput v2, VertexShaderOutput v3)
		{
			V1 = v1;
			V2 = v2;
			V3 = v3;
		}

		public IEnumerable<VertexShaderOutput> Vertices
		{
			get
			{
				yield return V1;
				yield return V2;
				yield return V3;
			}
		}
	}
}