using System.Collections.Generic;
using Nexus;
using Rasterizr.PipelineStages.Rasterizer;
using Rasterizr.PipelineStages.ShaderStages.Core;
using Rasterizr.PipelineStages.ShaderStages.VertexShader;

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
			foreach (var fragment in inputs)
			{
				ColorF color = PixelShader.Execute(fragment);
				Pixel pixel = _pixels[fragment.X, fragment.Y];
				pixel.Color = color;

				// Extract system-value attributes.
				pixel.Depth = fragment.Depth;

				// Copy sample data.
				pixel.Samples = fragment.Samples;

				outputs.Add(pixel);
			}
		}

		public IPixelShaderInput BuildPixelShaderInput()
		{
			return PixelShader.BuildPixelShaderInput();
		}
	}
}