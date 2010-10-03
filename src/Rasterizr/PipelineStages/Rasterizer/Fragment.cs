using System;
using Rasterizr.PipelineStages.TriangleSetup;

namespace Rasterizr.PipelineStages.Rasterizer
{
	/// <summary>
	/// A potential pixel.
	/// </summary>
	public class Fragment
	{
		public int X;
		public int Y;
		public float W; // Debugging only

		public InterpolatedVertexAttributeCollection Attributes;

		public Fragment(int x, int y)
		{
			X = x;
			Y = y;
			Attributes = new InterpolatedVertexAttributeCollection(RenderPipeline.MaxVertexAttributes);
			for (int i = 0; i < RenderPipeline.MaxVertexAttributes; ++i)
				Attributes.Add(new InterpolatedVertexAttribute());
		}

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