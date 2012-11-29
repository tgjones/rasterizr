using System.Linq;
using Rasterizr.Math;
using Rasterizr.Resources;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Xsgn;
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

		public void SetConstantBuffers(int startSlot, params Buffer[] constantBuffers)
		{
			throw new System.NotImplementedException();
		}

		public SamplerState[] GetSamplers(int startSlot, int count)
		{
			throw new System.NotImplementedException();
		}

		public void SetSamplers(int startSlot, params SamplerState[] samplers)
		{
			throw new System.NotImplementedException();
		}

		public ShaderResourceView[] GetShaderResources(int startSlot, int count)
		{
			throw new System.NotImplementedException();
		}

		public void SetShaderResources(int startSlot, params ShaderResourceView[] shaderResources)
		{
			throw new System.NotImplementedException();
		}
	}

	public abstract class CommonShaderStage<T> : CommonShaderStage
		where T : ShaderBase
	{
		private T _shader;
		private int _outputParametersCount;
		private VirtualMachine _virtualMachine;

		public T Shader
		{
			get { return _shader; }
			set
			{
				_shader = value;
				_outputParametersCount = value.Bytecode.OutputSignature.Parameters.Count;
				_virtualMachine = new VirtualMachine(value.Bytecode, NumShaderExecutionContexts);
				OnShaderChanged(value);
			}
		}

		protected virtual int NumShaderExecutionContexts
		{
			get { return 1; }
		}

		protected VirtualMachine VirtualMachine
		{
			get { return _virtualMachine; }
		}

		protected virtual void OnShaderChanged(T shader)
		{
			
		}

		protected void SetShaderInputs(int contextIndex, ushort primitiveIndex, Vector4[] inputs)
		{
			for (ushort i = 0; i < inputs.Length; i++)
				_virtualMachine.SetRegister(contextIndex, OperandType.Input, new RegisterIndex(primitiveIndex, i),
					inputs[i].ToNumber4());
		}

		protected Vector4[] GetShaderOutputs(int contextIndex)
		{
			var outputs = new Vector4[_outputParametersCount];
			for (ushort i = 0; i < outputs.Length; i++)
				outputs[i] = Vector4.FromNumber4(_virtualMachine.GetRegister(contextIndex, OperandType.Output, new RegisterIndex(i)));
			return outputs;
		}

		protected int? GetSystemValueRegister(Name systemValueType)
		{
			var parameter = Shader.Bytecode.OutputSignature.Parameters.SingleOrDefault(x => x.SystemValueType == systemValueType);
			if (parameter != null)
				return (int) parameter.Register;
			return null;
		}
	}
}