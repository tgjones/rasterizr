using SlimShader;

namespace Rasterizr.Pipeline
{
	public class ShaderBase : DeviceChild
	{
		private readonly BytecodeContainer _shaderBytecode;

		public BytecodeContainer Bytecode
		{
			get { return _shaderBytecode; }
		}

		public ShaderBase(Device device, BytecodeContainer shaderBytecode)
			: base(device)
		{
			_shaderBytecode = shaderBytecode;
		}
	}
}