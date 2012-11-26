using Rasterizr.Resources;
using SlimShader;
using SlimShader.Chunks.Shex;
using SlimShader.VirtualMachine;
using SlimShader.VirtualMachine.Registers;

namespace Rasterizr.Pipeline
{
	public abstract class CommonShaderStage
	{
		public Buffer[] GetConstantBuffers(int startSlot, int count)
		{
			throw new System.NotImplementedException();
		}

		public SamplerState[] GetSamplers(int startSlot, int count)
		{
			throw new System.NotImplementedException();
		}

		public ShaderResourceView[] GetShaderResources(int startSlot, int count)
		{
			throw new System.NotImplementedException();
		}
	}

	public abstract class CommonShaderStage<T> : CommonShaderStage
		where T : ShaderBase
	{
		private VirtualMachine _virtualMachine;
		private T _shader;

		public T Shader
		{
			get { return _shader; }
			set
			{
				_shader = value;
				_virtualMachine = new VirtualMachine(value.Bytecode, 1);
			}
		}

		protected void ExecuteShader(Number4[] inputs, Number4[] outputs)
		{
			for (ushort i = 0; i < inputs.Length; i++)
				_virtualMachine.GetRegister<NumberRegister>(0, OperandType.Input, new RegisterIndex(i)).Value = inputs[i];

			_virtualMachine.Execute();

			for (ushort i = 0; i < outputs.Length; i++)
				outputs[i] = _virtualMachine.GetRegister<NumberRegister>(0, OperandType.Output, new RegisterIndex(i)).Value;
		}
	}
}