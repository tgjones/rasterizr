using System.Collections.Concurrent;
using Nexus;
using Rasterizr.Rasterizer;

namespace Rasterizr.ShaderStages.PixelShader
{
	public class PixelShaderStage : PipelineStageBase<Fragment, Pixel>
	{
		public IPixelShader PixelShader { get; set; }

		public override void Run(BlockingCollection<Fragment> inputs, BlockingCollection<Pixel> outputs)
		{
			try
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
			}
			finally
			{
				outputs.CompleteAdding();
			}
		}

		public object BuildPixelShaderInput()
		{
			return PixelShader.BuildPixelShaderInput();
		}
	}
}