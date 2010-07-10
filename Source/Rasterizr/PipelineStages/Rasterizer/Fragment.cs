using System;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.TriangleSetup;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.Rasterizer
{
	/// <summary>
	/// A potential pixel.
	/// </summary>
	public struct Fragment
	{
		public int X;
		public int Y;
		public float W; // Debugging only

		public InterpolatedVertexAttribute[] Attributes;

		public override string ToString()
		{
			string result = string.Format("FRAGMENT DETAILS{0}W = {1}{0}", Environment.NewLine, W);
			foreach (InterpolatedVertexAttribute vertexAttribute in Attributes)
			{
				result += string.Format("{1} = {2}; d/dx = {3}; d/dy = {4}{0}", Environment.NewLine, vertexAttribute.Name,
					vertexAttribute.Value.Value, vertexAttribute.DValueDx.Value, vertexAttribute.DValueDy.Value);
			}
			return result;
		}
	}
}