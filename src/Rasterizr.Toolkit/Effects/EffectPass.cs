using Rasterizr.Pipeline.PixelShader;
using Rasterizr.Pipeline.VertexShader;

namespace Rasterizr.Toolkit.Effects
{
	public class EffectPass
	{
		private readonly EffectTechnique _parentTechnique;

		public VertexShader VertexShader { get; set; }
		public PixelShader PixelShader { get; set; }

		public EffectPass(EffectTechnique parentTechnique)
		{
			_parentTechnique = parentTechnique;
		}

		public void Apply()
		{
			_parentTechnique.ParentEffect.OnApply();

			_parentTechnique.ParentEffect.DeviceContext.VertexShader.Shader = VertexShader;
			_parentTechnique.ParentEffect.DeviceContext.PixelShader.Shader = PixelShader;
		}
	}
}