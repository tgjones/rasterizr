using Rasterizr.Core.ShaderCore;
using Rasterizr.Core.ShaderCore.PixelShader;

namespace Rasterizr.Effects
{
	public class EffectPass
	{
		private readonly EffectTechnique _parentTechnique;

		public IShader VertexShader { get; set; }
		public IPixelShader PixelShader { get; set; }

		public EffectPass(EffectTechnique parentTechnique)
		{
			_parentTechnique = parentTechnique;
		}

		public void Apply()
		{
			_parentTechnique.ParentEffect.OnApply();

			_parentTechnique.ParentEffect.Device.VertexShader.VertexShader = VertexShader;
			_parentTechnique.ParentEffect.Device.PixelShader.PixelShader = PixelShader;
		}
	}
}