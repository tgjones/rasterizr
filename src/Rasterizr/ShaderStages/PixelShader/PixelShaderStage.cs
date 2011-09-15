using System.Collections.Generic;
using Nexus;
using Rasterizr.Rasterizer;

namespace Rasterizr.ShaderStages.PixelShader
{
	public class PixelShaderStage : PipelineStageBase<Fragment, Pixel>
	{
		public IPixelShader PixelShader { get; set; }

		public override void Run(List<Fragment> inputs, List<Pixel> outputs)
		{
			foreach (var fragment in inputs)
			{
				ColorF color = PixelShader.Execute(fragment);
				var pixel = new Pixel(fragment.X, fragment.Y)
				{
					Color = color,
					Depth = fragment.Depth,
					Samples = fragment.Samples
				};
				outputs.Add(pixel);
			}
		}

		public object BuildPixelShaderInput()
		{
			return PixelShader.BuildPixelShaderInput();
		}
	}
}