using System.Collections.Generic;
using Nexus;
using Rasterizr.VertexAttributes;

namespace Rasterizr.PipelineStages.Rasterizer
{
	public struct TriangleSetupInfo
	{
		public Matrix2D VertexMatrixInverse;

		//public Interpolator<FloatVertexAttributeValue> WInterpolator;
		public Interpolator<FloatVertexAttributeValue> ZInterpolator;
		public Interpolator<FloatVertexAttributeValue> ConstantInterpolator;

		public Interpolator<FloatVertexAttributeValue> EdgeFunction1;
		public Interpolator<FloatVertexAttributeValue> EdgeFunction2;
		public Interpolator<FloatVertexAttributeValue> EdgeFunction3;

		public List<Interpolator> AttributeInterpolators;
	}
}