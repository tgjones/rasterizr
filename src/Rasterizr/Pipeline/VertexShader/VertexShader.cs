using Rasterizr.Diagnostics;

namespace Rasterizr.Pipeline.VertexShader
{
	public class VertexShader : ShaderBase
	{
		public VertexShader(Device device, byte[] shaderBytecode)
			: base(device, shaderBytecode)
		{
			device.Loggers.BeginOperation(OperationType.VertexShaderCreate, shaderBytecode);
		}
	}
}