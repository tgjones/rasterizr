using Rasterizr.Diagnostics;

namespace Rasterizr.Pipeline.PixelShader
{
	public class PixelShader : ShaderBase
	{
		public PixelShader(Device device, byte[] shaderBytecode) 
			: base(device, shaderBytecode)
		{
			device.Loggers.BeginOperation(OperationType.PixelShaderCreate, shaderBytecode);
		}
	}
}