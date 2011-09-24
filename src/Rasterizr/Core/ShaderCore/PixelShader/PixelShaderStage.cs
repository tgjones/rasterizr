using System.Collections.Generic;
using Nexus;
using Rasterizr.Core.Rasterizer;

namespace Rasterizr.Core.ShaderCore.PixelShader
{
	public class PixelShaderStage : PipelineStageBase<Fragment, Pixel>
	{
		public IShader PixelShader { get; set; }

		public override void Run(List<Fragment> inputs, List<Pixel> outputs)
		{
			foreach (var fragment in inputs)
			{
				ColorF color = (ColorF) PixelShader.Execute(fragment.PixelShaderInput);
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
			return PixelShader.BuildShaderInput();
		}
	}
}