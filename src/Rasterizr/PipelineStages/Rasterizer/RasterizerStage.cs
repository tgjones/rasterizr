using System.Collections.Generic;
using Rasterizr.PipelineStages.TriangleSetup;

namespace Rasterizr.PipelineStages.Rasterizer
{
	public class RasterizerStage : PipelineStageBase<Triangle, Fragment>
	{
		private readonly Fragment[,] _fragments;

		public CullMode CullMode { get; set; }

		public RasterizerStage(Viewport3D viewport)
		{
			_fragments = new Fragment[viewport.Width, viewport.Height];
			for (int y = 0; y < viewport.Height; ++y)
				for (int x = 0; x < viewport.Width; ++x)
					_fragments[x, y] = new Fragment(x, y);

			CullMode = CullMode.CullCounterClockwiseFace;
		}

		public override void Process(IList<Triangle> inputs, IList<Fragment> outputs)
		{
			foreach (Triangle triangle in inputs)
			{
				// Apply culling mode.

				ProcessTriangle(triangle, outputs);
			}
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
				// TODO: Remove this check once culling and clipping are implemented.
				if (x >= 0 && x < _fragments.GetLength(0) && scanline.Y >= 0 && scanline.Y < _fragments.GetLength(1))
				{
					Fragment fragment = _fragments[x, scanline.Y];
					fragment.Attributes.ActiveLength = attributes.Attributes.Length;

					float w = 1 / oneOverW;
					for (int i = 0; i < attributes.Attributes.Length; ++i)
					{
						fragment.Attributes[i] = new InterpolatedVertexAttribute
						{
							Name = attributes.Attributes[i].Name,
							Semantic = attributes.Attributes[i].Semantic,
							InterpolationType = attributes.Attributes[i].InterpolationType,
							Value = attributes.Attributes[i].GetValue(w),
							DValueDx = triangle.VertexAttributeXStepValues[i].Multiply(w),
							// Constant over the triangle, so could be moved to a higher level entity.
							DValueDy = triangle.VertexAttributeYStepValues[i].Multiply(w)
						};
					}

					fragment.W = oneOverW;
					outputs.Add(fragment);
				}

				oneOverW += triangle.DOneOverWdX;
				for (int i = 0; i < attributes.Attributes.Length; ++i)
					attributes.Attributes[i].Value = attributes.Attributes[i].Value.Add(triangle.VertexAttributeXStepValues[i]);
			}
		}
	}
}