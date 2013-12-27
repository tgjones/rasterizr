using System.Linq;
using Rasterizr.Resources;
using Rasterizr.Util;
using SlimShader;
using SlimShader.Chunks.Rdef;
using SlimShader.Chunks.Xsgn;
using SlimShader.VirtualMachine;
using SlimShader.VirtualMachine.Registers;

namespace Rasterizr.Pipeline
{
    public abstract class CommonShaderStage<T>
		where T : ShaderBase
	{
        public event DiagnosticEventHandler SettingConstantBuffers;
        public event DiagnosticEventHandler SettingSamplers;
        public event DiagnosticEventHandler SettingShader;
        public event DiagnosticEventHandler SettingShaderResources;

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
                DiagnosticUtilities.RaiseEvent(this, SettingShader, DiagnosticUtilities.GetID(value));

				_shader = value;
			    if (value != null)
			    {
			        _outputParametersCount = value.Bytecode.OutputSignature.Parameters.Count;
			        _virtualMachine = new VirtualMachine(value.Bytecode, BatchSize);
			    }
			    else
			    {
			        _outputParametersCount = 0;
			        _virtualMachine = null;
			    }
			    OnShaderChanged(value);
			}
		}

		protected virtual int BatchSize
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
            DiagnosticUtilities.RaiseEvent(this, SettingConstantBuffers, startSlot, DiagnosticUtilities.GetIDs(constantBuffers));
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
            DiagnosticUtilities.RaiseEvent(this, SettingSamplers, startSlot, DiagnosticUtilities.GetIDs(samplers));
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
            DiagnosticUtilities.RaiseEvent(this, SettingShaderResources, startSlot, DiagnosticUtilities.GetIDs(shaderResources));
		    if (shaderResources != null)
		        for (int i = 0; i < shaderResources.Length; i++)
		            _shaderResources[i + startSlot] = shaderResources[i];
		    else
		        _shaderResources[startSlot] = null;
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
                    for (ushort j = 0; j < System.Math.Max((int) variableDefinition.ShaderType.ElementCount, 1); j++) // Element count is 0 if not an array.
					    for (ushort k = 0; k < variableDefinition.ShaderType.Rows; k++)
					    {
						    Number4 value;
						    _constantBuffers[i].GetData(out value, 
							    (int) variableDefinition.StartOffset + (j * variableDefinition.ShaderType.Rows * variableDefinition.ShaderType.Columns * 4) + (k * variableDefinition.ShaderType.Columns * 4),
							    variableDefinition.ShaderType.Columns * 4);
						    _virtualMachine.SetConstantBufferRegisterValue(
							    i, registerIndex++, ref value);
					    }
			}

			// TODO: Get texture count from virtual machine.
            for (ushort i = 0; i < _shader.Bytecode.ResourceDefinition.ResourceBindings.Count(x => x.Type == ShaderInputType.Sampler); i++)
                _virtualMachine.SetSampler(new RegisterIndex(i),
                    (_samplers[i] != null)
                        ? _samplers[i].VirtualMachineSamplerState
                        : null);

			for (ushort i = 0; i < _shader.Bytecode.ResourceDefinition.ResourceBindings.Count(x => x.Type == ShaderInputType.Texture); i++)
				_virtualMachine.SetTexture(new RegisterIndex(i),
					(_shaderResources[i] != null)
						? _shaderResources[i].InnerView
						: null);
		}

		protected void SetShaderInputs(int contextIndex, ushort primitiveIndex, Number4[] inputs)
		{
			for (ushort i = 0; i < inputs.Length; i++)
				_virtualMachine.SetInputRegisterValue(contextIndex, primitiveIndex, i, ref inputs[i]);
		}

		protected Number4[] GetShaderOutputs(int contextIndex)
		{
			var outputs = new Number4[_outputParametersCount];
			for (ushort i = 0; i < outputs.Length; i++)
				outputs[i] = _virtualMachine.GetOutputRegisterValue(contextIndex, i);
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