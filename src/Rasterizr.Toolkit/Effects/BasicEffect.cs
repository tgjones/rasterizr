namespace Rasterizr.Toolkit.Effects
{
	public class BasicEffect : Effect
	{
		public BasicEffect(DeviceContext deviceContext)
			: base(deviceContext, BasicEffectCode.VertexShaderCode, BasicEffectCode.PixelShaderCode)
		{
		}
	}
}