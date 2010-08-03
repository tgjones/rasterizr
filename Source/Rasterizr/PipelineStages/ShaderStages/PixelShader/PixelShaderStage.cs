using System.Collections.Generic;
using Nexus;
using Rasterizr.PipelineStages.Rasterizer;

namespace Rasterizr.PipelineStages.ShaderStages.PixelShader
{
	public class PixelShaderStage : PipelineStageBase<Fragment, Pixel>
	{
		private readonly Pixel[,] _pixels;

		public IPixelShader PixelShader { get; set; }

		public PixelShaderStage(Viewport3D viewport)
		{
			_pixels = new Pixel[viewport.Width, viewport.Height];
			for (int y = 0; y < viewport.Height; ++y)
				for (int x = 0; x < viewport.Width; ++x)
					_pixels[x, y] = new Pixel(x, y);
		}

		public override void Process(IList<Fragment> inputs, IList<Pixel> outputs)
		{
			for (int i = 0, length = inputs.Count; i < length; ++i)
			{
				Color color = PixelShader.Execute(inputs[i]);
				Pixel pixel = _pixels[inputs[i].X, inputs[i].Y];
				pixel.Color = color;
				outputs.Add(pixel);
			}
		}
	}
}