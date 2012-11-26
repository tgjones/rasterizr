using System.Collections.Generic;
using Rasterizr.Math;
using Rasterizr.Pipeline.Rasterizer;

namespace Rasterizr.Pipeline.PixelShader
{
	public class PixelShaderStage : CommonShaderStage<PixelShader>
	{
		internal IEnumerable<Pixel> Execute(IEnumerable<FragmentQuad> inputs)
		{
			// Process groups of four fragments together.
			foreach (var fragmentQuad in inputs)
			{
				yield return new Pixel(fragmentQuad.Fragment0.X, fragmentQuad.Fragment0.Y) { Color = new Color4F(1, 0, 0, 1) };
				yield return new Pixel(fragmentQuad.Fragment1.X, fragmentQuad.Fragment1.Y) { Color = new Color4F(1, 0, 0, 1) };
				yield return new Pixel(fragmentQuad.Fragment2.X, fragmentQuad.Fragment2.Y) { Color = new Color4F(1, 0, 0, 1) };
				yield return new Pixel(fragmentQuad.Fragment3.X, fragmentQuad.Fragment3.Y) { Color = new Color4F(1, 0, 0, 1) };
			}
		}
	}
}