using Rasterizr.PipelineStages.ShaderStages.PixelShader;
using Rasterizr.PipelineStages.ShaderStages.VertexShader;

namespace Rasterizr.PipelineStages.ShaderStages.Core
{
	public class EffectPass
	{
		private readonly EffectTechnique _parentTechnique;

		public IVertexShader VertexShader { get; set; }
		public IPixelShader PixelShader { get; set; }

		public EffectPass(EffectTechnique parentTechnique)
		{
			_parentTechnique = parentTechnique;
		}

		public void Apply()
		{
			_parentTechnique.ParentEffect.OnApply();

			_parentTechnique.ParentEffect.Device.RenderPipeline.VertexShader.VertexShader = VertexShader;
			_parentTechnique.ParentEffect.Device.RenderPipeline.PixelShader.PixelShader = PixelShader;
		}
	}
}