using System.Collections.Concurrent;
using Nexus;
using Rasterizr.PipelineStages.Rasterizer;

namespace Rasterizr.PipelineStages.ShaderStages.PixelShader
{
	public class PixelShaderStage : PipelineStageBase<Fragment, Pixel>
	{
		public IPixelShader PixelShader { get; set; }

		public override void Run(BlockingCollection<Fragment> inputs, BlockingCollection<Pixel> outputs)
		{
			foreach (var fragment in inputs.GetConsumingEnumerable())
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
			outputs.CompleteAdding();
		}

		public IPixelShaderInput BuildPixelShaderInput()
		{
			return PixelShader.BuildPixelShaderInput();
		}
	}
}