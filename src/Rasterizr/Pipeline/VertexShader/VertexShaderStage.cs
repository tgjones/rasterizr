using System.Collections.Generic;
using Rasterizr.Pipeline.InputAssembler;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Pipeline.VertexShader
{
	public class VertexShaderStage : CommonShaderStage<VertexShader>
	{
		private int? _outputPositionRegister;

		protected override void OnShaderChanged(VertexShader shader)
		{
			_outputPositionRegister = GetSystemValueRegister(Name.Position);
			base.OnShaderChanged(shader);
		}

		internal IEnumerable<VertexShaderOutput> Execute(IEnumerable<InputAssemblerVertexOutput> inputs)
		{
			foreach (var input in inputs)
			{
				SetShaderInputs(0, 0, input.Data);
				VirtualMachine.Execute();
				var outputs = GetShaderOutputs(0);

				var result = new VertexShaderOutput
				{
					VertexID = input.VertexID,
					InstanceID = input.InstanceID,
					Data = outputs
				};
				if (_outputPositionRegister != null)
					result.Position = outputs[_outputPositionRegister.Value];

				yield return result;
			}
		}
	}
}