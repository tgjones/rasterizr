using SlimShader;

namespace Rasterizr.Pipeline.PixelShader
{
	public class PixelShader : ShaderBase
	{
		public PixelShader(Device device, BytecodeContainer shaderBytecode) 
			: base(device, shaderBytecode)
		{
		}
	}
}