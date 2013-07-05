using System.Linq;
using Rasterizr.Math;
using Rasterizr.Resources;
using SlimShader.Chunks.Rdef;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Xsgn;
using SlimShader.VirtualMachine;
using SlimShader.VirtualMachine.Registers;

namespace Rasterizr.Pipeline
{
	public abstract class CommonShaderStage<T>
		where T : ShaderBase
	{
		private readonly Device _device;
		public const int ConstantBufferSlotCount = 14;
		public const int SamplerSlotCount = 16;
		public const int ShaderResourceSlotCount = 128;

		private readonly Buffer[] _constantBuffers;
		private readonly SamplerState[] _samplers;
		private readonly ShaderResourceView[] _shaderResources;

		private T _shader;
		private int _outputParametersCount;
		private VirtualMachine _virtualMachine;

		protected Device Device
		{
			get { return _device; }
		}

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

		protected CommonShaderStage(Device device)
		{
			_device = device;
			_constantBuffers = new Buffer[ConstantBufferSlotCount];
			_samplers = new SamplerState[SamplerSlotCount];
			_shaderResources = new ShaderResourceView[ShaderResourceSlotCount];
		}

		public void GetConstantBuffers(int startSlot, int count, Buffer[] constantBuffers)
		{
			for (int i = 0; i < count; i++)
				constantBuffers[i] = _constantBuffers[i + startSlot];
		}

		public void SetConstantBuffers(int startSlot, params Buffer[] constantBuffers)
		{
			for (int i = 0; i < constantBuffers.Length; i++)
				_constantBuffers[i + startSlot] = constantBuffers[i];
		}

		public void GetSamplers(int startSlot, int count, SamplerState[] samplers)
		{
			for (int i = 0; i < count; i++)
				samplers[i] = _samplers[i + startSlot];
		}

		public void SetSamplers(int startSlot, params SamplerState[] samplers)
		{
			for (int i = 0; i < samplers.Length; i++)
				_samplers[i + startSlot] = samplers[i];
		}

		public void GetShaderResources(int startSlot, int count, ShaderResourceView[] shaderResources)
		{
			for (int i = 0; i < count; i++)
				shaderResources[i] = _shaderResources[i + startSlot];
		}

		public void SetShaderResources(int startSlot, params ShaderResourceView[] shaderResources)
		{
			for (int i = 0; i < shaderResources.Length; i++)
				_shaderResources[i + startSlot] = shaderResources[i];
		}

		protected virtual void OnShaderChanged(T shader)
		{
			
		}

		protected void SetShaderConstants()
		{
			for (ushort i = 0; i < _shader.Bytecode.ResourceDefinition.ConstantBuffers.Count; i++)
			{
				ushort registerIndex = 0;
				var constantBufferDefinition = _shader.Bytecode.ResourceDefinition.ConstantBuffers[i];
				foreach (var variableDefinition in constantBufferDefinition.Variables)
					for (ushort k = 0; k < variableDefinition.ShaderType.Rows; k++)
					{
						Vector4 value;
						_constantBuffers[i].GetData(out value, 
							(int) variableDefinition.StartOffset + k * variableDefinition.ShaderType.Columns * 4,
							variableDefinition.ShaderType.Columns * 4);
						_virtualMachine.SetRegister(0, OperandType.ConstantBuffer, 
							new RegisterIndex(i, registerIndex++), value.ToNumber4());
					}
			}

			// TODO: Get texture count from virtual machine.
            for (ushort i = 0; i < _shader.Bytecode.ResourceDefinition.ResourceBindings.Count(x => x.Type == ShaderInputType.Sampler); i++)
                _virtualMachine.SetSampler(new RegisterIndex(i),
                    (_samplers[i] != null)
                        ? _samplers[i].InnerSampler
                        : null);

			for (ushort i = 0; i < _shader.Bytecode.ResourceDefinition.ResourceBindings.Count(x => x.Type == ShaderInputType.Texture); i++)
				_virtualMachine.SetTexture(new RegisterIndex(i),
					(_shaderResources[i] != null)
						? _shaderResources[i].InnerView
						: null);
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