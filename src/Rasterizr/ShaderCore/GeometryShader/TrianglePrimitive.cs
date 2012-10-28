using System.Collections.Generic;

namespace Rasterizr.ShaderCore.GeometryShader
{
	public struct TrianglePrimitive
	{
		public TransformedVertex V1;
		public TransformedVertex V2;
		public TransformedVertex V3;

		public TrianglePrimitive(TransformedVertex v1, TransformedVertex v2, TransformedVertex v3)
		{
			V1 = v1;
			V2 = v2;
			V3 = v3;
		}

		public IEnumerable<TransformedVertex> Vertices
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