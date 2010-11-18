using Nexus;
using Rasterizr.VertexAttributes;

namespace Rasterizr.PipelineStages.Rasterizer
{
	public static class InterpolatorUtility
	{
		public static Interpolator CreateInterpolator(Matrix2D matrix, IVertexAttributeValue value1, IVertexAttributeValue value2, IVertexAttributeValue value3)
		{
			return new Interpolator
			{
				Edge1 = value1.Multiply(matrix.M11).Add(value2.Multiply(matrix.M21)).Add(value3.Multiply(matrix.M31)),
				Edge2 = value1.Multiply(matrix.M12).Add(value2.Multiply(matrix.M22)).Add(value3.Multiply(matrix.M32)),
				Edge3 = value1.Multiply(matrix.M13).Add(value2.Multiply(matrix.M23)).Add(value3.Multiply(matrix.M33))
			};
		}

		public static Interpolator<FloatVertexAttributeValue> CreateInterpolator(Matrix2D matrix, float value1, float value2, float value3)
		{
			return new Interpolator<FloatVertexAttributeValue>
			{
				Edge1 = new FloatVertexAttributeValue { Value = value1 * matrix.M11 + value2 * matrix.M21 + value3 * matrix.M31 },
				Edge2 = new FloatVertexAttributeValue { Value = value1 * matrix.M12 + value2 * matrix.M22 + value3 * matrix.M32 },
				Edge3 = new FloatVertexAttributeValue { Value = value1 * matrix.M13 + value2 * matrix.M23 + value3 * matrix.M33 }
			};
		}
	}
}