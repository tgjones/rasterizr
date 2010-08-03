using System;
using Rasterizr.PipelineStages.PerspectiveDivide;

namespace Rasterizr.PipelineStages.TriangleSetup
{
	public struct TriangleGradients
	{
		public VertexAttributeStepValues[] VertexAttributeStepValues;

		/// <summary>
		/// One for each of the three vertices
		/// </summary>
		public InterpolatedVertexAttributes[] InterpolatedVertexAttributes;

		/// <summary>
		/// d(1/w)/dX
		/// </summary>
		public float DOneOverWdX;

		/// <summary>
		/// d(1/w)/dY
		/// </summary>
		public float DOneOverWdY;

		public TriangleGradients(ScreenSpaceVertex[] vertices)
		{
			float oneOverdX = 1 / (((vertices[1].Position.X - vertices[2].Position.X) * (vertices[0].Position.Y - vertices[2].Position.Y)) - ((vertices[0].Position.X - vertices[2].Position.X) * (vertices[1].Position.Y - vertices[2].Position.Y)));
			float oneOverdY = -oneOverdX;

			// Setup per-vertex interpolants.
			InterpolatedVertexAttributes = new InterpolatedVertexAttributes[3];
			for (int i = 0; i < 3; ++i)
			{
				// We always interpolate 1/w, even though there may not be any vertex attributes that require perspective interpolation.
				// I assume it's not worth the computational effort to check.
				float oneOverW = 1.0f / vertices[i].W;
				InterpolatedVertexAttributes[i].OneOverW = oneOverW;

				// Setup interpolation parameters for each vertex attribute, based on the specified interpolation type.
				InterpolatedVertexAttributes[i].Attributes = new InterpolatedVertexAttribute[vertices[i].Attributes.Length];
				for (int j = 0; j < vertices[i].Attributes.Length; ++j)
				{
					InterpolatedVertexAttributes[i].Attributes[j] = new InterpolatedVertexAttribute
					{
						Name = vertices[i].Attributes[j].Name,
						InterpolationType = vertices[i].Attributes[j].InterpolationType
					};
					InterpolatedVertexAttributes[i].Attributes[j].SetValue(vertices[i].Attributes[j].Value, oneOverW);
				}
			}

			// Setup partial derivatives.

			DOneOverWdX = oneOverdX * (((InterpolatedVertexAttributes[1].OneOverW - InterpolatedVertexAttributes[2].OneOverW) * (vertices[0].Position.Y - vertices[2].Position.Y)) - ((InterpolatedVertexAttributes[0].OneOverW - InterpolatedVertexAttributes[2].OneOverW) * (vertices[1].Position.Y - vertices[2].Position.Y)));
			DOneOverWdY = oneOverdY * (((InterpolatedVertexAttributes[1].OneOverW - InterpolatedVertexAttributes[2].OneOverW) * (vertices[0].Position.X - vertices[2].Position.X)) - ((InterpolatedVertexAttributes[0].OneOverW - InterpolatedVertexAttributes[2].OneOverW) * (vertices[1].Position.X - vertices[2].Position.X)));

			VertexAttributeStepValues = new VertexAttributeStepValues[vertices[0].Attributes.Length];
			for (int i = 0; i < vertices[0].Attributes.Length; ++i)
			{
				VertexAttributeStepValues[i].InterpolationType = vertices[0].Attributes[i].InterpolationType;
				VertexAttributeStepValues[i].SetValues(
					InterpolatedVertexAttributes[0].Attributes[i].Value,
					InterpolatedVertexAttributes[1].Attributes[i].Value,
					InterpolatedVertexAttributes[2].Attributes[i].Value,
					vertices[0].Position,
					vertices[1].Position,
					vertices[2].Position,
					oneOverdX,
					oneOverdY);
			}
		}

		public override string ToString()
		{
			string result = string.Format("DOneOverWdX = {1}{0}DOneOverWdY = {2}{0}{0}Interpolated Vertex Attributes:{0}",
				Environment.NewLine, DOneOverWdX, DOneOverWdY);
			for (int i = 0; i < InterpolatedVertexAttributes.Length; ++i)
			{
				result += string.Format("Vertex {1}{0}OneOverW = {2}{0}", Environment.NewLine, i, InterpolatedVertexAttributes[i].OneOverW);
				for (int j = 0; j < InterpolatedVertexAttributes[i].Attributes.Length; ++j)
					result += string.Format("Attribute {1} ({2}) = {3}{0}", Environment.NewLine, j, InterpolatedVertexAttributes[i].Attributes[j].Name,
						InterpolatedVertexAttributes[i].Attributes[j].Value.Value);
				result += Environment.NewLine;
			}
			return result;
		}
	}
}