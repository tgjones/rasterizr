using System.Collections.Generic;
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
		private T _shader;
		private VirtualMachine _virtualMachine;

		public T Shader
		{
			get { return _shader; }
			set
			{
				_shader = value;
				_virtualMachine = new VirtualMachine(value.Bytecode, 1);
				OnShaderChanged(value);
			}
		}

		protected virtual void OnShaderChanged(T shader)
		{
			
		}

		protected void SetShaderInputs(ushort primitiveIndex, Vector4[] inputs)
		{
			for (ushort i = 0; i < inputs.Length; i++)
				_virtualMachine.SetRegister(0, OperandType.Input, new RegisterIndex(primitiveIndex, i), inputs[i].ToNumber4());
		}

		protected IEnumerable<ExecutionResponse> ExecuteShaderMultiple()
		{
			return _virtualMachine.Execute();
		}

		protected void ExecuteShader()
		{
			ExecuteShaderMultiple().ToList();
		}

		protected Vector4[] GetShaderOutputs()
		{
			var outputs = new Vector4[Shader.Bytecode.OutputSignature.Parameters.Count];
			for (ushort i = 0; i < outputs.Length; i++)
				outputs[i] = Vector4.FromNumber4(_virtualMachine.GetRegister(0, OperandType.Output, new RegisterIndex(i)));
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