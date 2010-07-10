using System.Collections.Generic;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.Rasterizer;
using Nexus;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.PixelShader
{
	public class PixelShaderStage : PipelineStageBase<Fragment, Pixel>
	{
		public IPixelShader PixelShader { get; set; }

		public override void Process(IList<Fragment> inputs, IList<Pixel> outputs)
		{
			for (int i = 0, length = inputs.Count; i < length; ++i)
			{
				Color color = PixelShader.Execute(inputs[i]);
				outputs.Add(new Pixel
				{
					X = inputs[i].X,
					Y = inputs[i].Y,
					Color = color
				});
			}
		}
	}
}