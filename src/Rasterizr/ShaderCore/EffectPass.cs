namespace Rasterizr.ShaderCore
{
	public class EffectPass
	{
		private readonly EffectTechnique _parentTechnique;

		public IShader VertexShader { get; set; }
		public IShader PixelShader { get; set; }

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