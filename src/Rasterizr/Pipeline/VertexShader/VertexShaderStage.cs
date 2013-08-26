using System.Collections.Generic;
using Rasterizr.Pipeline.InputAssembler;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Pipeline.VertexShader
{
	public class VertexShaderStage : CommonShaderStage<VertexShader>
	{
		private int? _outputPositionRegister;

		public VertexShaderStage(Device device)
			: base(device)
		{

		}

		protected override void OnShaderChanged(VertexShader shader)
		{
			_outputPositionRegister = GetSystemValueRegister(Name.Position);
			base.OnShaderChanged(shader);
		}

		internal IEnumerable<VertexShaderOutput> Execute(IEnumerable<InputAssemblerVertexOutput> inputs)
		{
			SetShaderConstants();

			foreach (var input in inputs)
			{
				SetShaderInputs(0, 0, input.Data);
				VirtualMachine.Execute();
				var outputs = GetShaderOutputs(0);

				var result = new VertexShaderOutput
				{
					VertexID = input.VertexID,
					InstanceID = input.InstanceID,
                    InputData = input.Data,
					OutputData = outputs
				};
				if (_outputPositionRegister != null)
					result.Position = outputs[_outputPositionRegister.Value];

				yield return result;
			}
		}
	}
}