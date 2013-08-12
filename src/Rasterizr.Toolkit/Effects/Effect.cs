using Rasterizr.Pipeline.PixelShader;
using Rasterizr.Pipeline.VertexShader;

namespace Rasterizr.Toolkit.Effects
{
	public class Effect
	{
		public DeviceContext DeviceContext { get; private set; }

        public VertexShader VertexShader { get; private set; }
        public PixelShader PixelShader { get; private set; }

		public Effect(DeviceContext deviceContext,
            byte[] vertexShaderBytecode,
            byte[] pixelShaderBytecode)
		{
			DeviceContext = deviceContext;

		    VertexShader = deviceContext.Device.CreateVertexShader(vertexShaderBytecode);
		    PixelShader = deviceContext.Device.CreatePixelShader(pixelShaderBytecode);
		}

		public virtual void Apply()
		{
            DeviceContext.VertexShader.Shader = VertexShader;
            DeviceContext.PixelShader.Shader = PixelShader;
		}
	}
}