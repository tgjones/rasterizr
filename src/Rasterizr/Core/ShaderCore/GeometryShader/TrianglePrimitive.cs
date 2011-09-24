using System.Collections.Generic;
using Rasterizr.Core.ShaderCore.VertexShader;

namespace Rasterizr.Core.ShaderCore.GeometryShader
{
	public struct TrianglePrimitive
	{
		public IVertexShaderOutput V1;
		public IVertexShaderOutput V2;
		public IVertexShaderOutput V3;

		public TrianglePrimitive(IVertexShaderOutput v1, IVertexShaderOutput v2, IVertexShaderOutput v3)
		{
			V1 = v1;
			V2 = v2;
			V3 = v3;
		}

		public IEnumerable<IVertexShaderOutput> Vertices
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