using Rasterizr.Diagnostics;
using SlimShader;

namespace Rasterizr.Pipeline.VertexShader
{
	public class VertexShader : ShaderBase
	{
		public VertexShader(Device device, BytecodeContainer shaderBytecode)
			: base(device, shaderBytecode)
		{
			device.Loggers.BeginOperation(OperationType.VertexShaderCreate, shaderBytecode);
		}
	}
}