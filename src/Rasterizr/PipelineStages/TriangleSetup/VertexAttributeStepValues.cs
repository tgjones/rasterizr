using Nexus;
using Rasterizr.VertexAttributes;

namespace Rasterizr.PipelineStages.TriangleSetup
{
	public struct VertexAttributeStepValues
	{
		public VertexAttributeInterpolationType InterpolationType;
		public IVertexAttributeValue XStep;
		public IVertexAttributeValue YStep;

		public void SetValues(
			IVertexAttributeValue v1Value, IVertexAttributeValue v2Value, IVertexAttributeValue v3Value,
			Point3D p1, Point3D p2, Point3D p3,
			float oneOverdX, float oneOverdY)
		{
			XStep = (((v2Value.Subtract(v3Value)).Multiply(p1.Y - p3.Y)).Subtract((v1Value.Subtract(v3Value)).Multiply(p2.Y - p3.Y))).Multiply(oneOverdX);
			YStep = (((v2Value.Subtract(v3Value)).Multiply(p1.X - p3.X)).Subtract((v1Value.Subtract(v3Value)).Multiply(p2.X - p3.X))).Multiply(oneOverdY);
		}
	}
}