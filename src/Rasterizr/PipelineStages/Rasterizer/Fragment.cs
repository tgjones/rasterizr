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
		public SampleCollection Samples;

		public InterpolatedVertexAttributeCollection Attributes;

		public Fragment(int x, int y)
		{
			X = x;
			Y = y;
			Attributes = new InterpolatedVertexAttributeCollection(RenderPipeline.MaxVertexAttributes);
			for (int i = 0; i < RenderPipeline.MaxVertexAttributes; ++i)
				Attributes.Add(new InterpolatedVertexAttribute());
			Samples = new SampleCollection();
		}

		public override string ToString()
		{
			string result = string.Format("FRAGMENT DETAILS{0}", Environment.NewLine);
			foreach (InterpolatedVertexAttribute vertexAttribute in Attributes)
			{
				result += string.Format("{1} = {2}; d/dx = {3}; d/dy = {4}{0}", Environment.NewLine, vertexAttribute.Name,
					vertexAttribute.Value.Value, vertexAttribute.DValueDx.Value, vertexAttribute.DValueDy.Value);
			}
			return result;
		}
	}
}