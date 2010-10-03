using System;
using Rasterizr.VertexAttributes;

namespace Rasterizr.PipelineStages.TriangleSetup
{
	public struct InterpolatedVertexAttribute
	{
		public string Name;
		public string Semantic;
		public VertexAttributeInterpolationType InterpolationType;
		public IVertexAttributeValue Value;

		// Used by pixel shader for texture mipmapping.
		/// <summary>
		/// Change in value per horizontal pixel step
		/// </summary>
		public IVertexAttributeValue DValueDx;

		/// <summary>
		/// Change in value per vertical pixel step.
		/// </summary>
		public IVertexAttributeValue DValueDy;

		public void SetValue(IVertexAttributeValue value, float oneOverW)
		{
			switch (InterpolationType)
			{
				case VertexAttributeInterpolationType.Perspective :
					Value = value.Multiply(oneOverW);
					break;
				case VertexAttributeInterpolationType.Linear :
					Value = value;
					break;
				default:
					throw new NotSupportedException();
			}
		}

		public IVertexAttributeValue GetValue(float w)
		{
			switch (InterpolationType)
			{
				case VertexAttributeInterpolationType.Perspective:
					return Value.Multiply(w);
				case VertexAttributeInterpolationType.Linear:
					return Value;
				default :
					throw new NotSupportedException();
			}
		}
	}
}