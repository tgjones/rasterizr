using System.Collections.Generic;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.TriangleSetup;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.Rasterizer
{
	public class RasterizerStage : PipelineStageBase<Triangle, Fragment>
	{
		public override void Process(IList<Triangle> inputs, IList<Fragment> outputs)
		{
			foreach (Triangle triangle in inputs)
				ProcessTriangle(triangle, outputs);
		}

		internal void ProcessTriangle(Triangle triangle, IList<Fragment> outputs)
		{
			foreach (Scanline scanline in triangle.Scanlines)
				ProcessScanline(triangle, scanline, outputs);
		}

		internal void ProcessScanline(Triangle triangle, Scanline scanline, IList<Fragment> outputs)
		{
			float oneOverW = scanline.InterpolatedVertexAttributes.OneOverW;
			InterpolatedVertexAttributes attributes = scanline.InterpolatedVertexAttributes;
			for (int x = scanline.XStart; x <= scanline.XStart + scanline.Width; ++x)
			{
				float w = 1 / oneOverW;
				InterpolatedVertexAttribute[] interpolatedAttributes = new InterpolatedVertexAttribute[attributes.Attributes.Length];
				for (int i = 0; i < attributes.Attributes.Length; ++i)
				{
					interpolatedAttributes[i].Name = attributes.Attributes[i].Name;
					interpolatedAttributes[i].InterpolationType = attributes.Attributes[i].InterpolationType;
					interpolatedAttributes[i].Value = attributes.Attributes[i].GetValue(w);

					// Constant over the triangle, so could be moved to a higher level entity.
					interpolatedAttributes[i].DValueDx = triangle.VertexAttributeXStepValues[i].Multiply(w);
					interpolatedAttributes[i].DValueDy = triangle.VertexAttributeYStepValues[i].Multiply(w);
				}

				outputs.Add(new Fragment
				{
					X = x,
					Y = scanline.Y,
					Attributes = interpolatedAttributes,
					W = oneOverW
				});

				oneOverW += triangle.DOneOverWdX;
				for (int i = 0; i < attributes.Attributes.Length; ++i)
					attributes.Attributes[i].Value = attributes.Attributes[i].Value.Add(triangle.VertexAttributeXStepValues[i]);
			}
		}
	}
}