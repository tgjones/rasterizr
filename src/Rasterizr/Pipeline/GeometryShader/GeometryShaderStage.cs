using System;
using System.Collections.Generic;
using Rasterizr.Diagnostics;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.VertexShader;
using Rasterizr.Util;
using SlimShader.Chunks.Xsgn;
using SlimShader.VirtualMachine;

namespace Rasterizr.Pipeline.GeometryShader
{
	public class GeometryShaderStage : CommonShaderStage<GeometryShader>
	{
		private int _outputPositionRegister;
		private PrimitiveTopology _outputTopology;

		internal PrimitiveTopology OutputTopology
		{
			get { return _outputTopology; }
		}

		public bool IsActive
		{
			get { return Shader != null; }
		}

        protected override OperationType SetConstantBuffersOperationType
        {
            get { return OperationType.GeometryShaderStageSetConstantBuffers; }
        }

        protected override OperationType SetSamplersOperationType
        {
            get { return OperationType.GeometryShaderStageSetSamplers; }
        }

        protected override OperationType SetShaderResourcesOperationType
        {
            get { return OperationType.GeometryShaderStageSetShaderResources; }
        }

		public GeometryShaderStage(Device device)
			: base(device)
		{

		}

		protected override void OnShaderChanged(GeometryShader shader)
		{
			Device.Loggers.BeginOperation(OperationType.GeometryShaderStageSetShader, shader);

			var outputPositionRegister = GetSystemValueRegister(Name.Position);
			if (outputPositionRegister == null)
				throw new ArgumentException("Shader doesn't contain output position", "shader");
			_outputPositionRegister = outputPositionRegister.Value;

			_outputTopology = shader.Bytecode.Statistics.GeometryShaderOutputTopology.ToPrimitiveTopology();

			base.OnShaderChanged(shader);
		}

		internal IEnumerable<InputAssemblerPrimitiveOutput> Execute(IEnumerable<InputAssemblerPrimitiveOutput> inputs, PrimitiveTopology primitiveTopology)
		{
			SetShaderConstants();

			foreach (var input in inputs)
			{
				for (ushort i = 0; i < input.Vertices.Length; i++)
					SetShaderInputs(0, i, input.Vertices[i].Data);

				foreach (var primitive in PrimitiveAssembler.GetPrimitiveStream(GetVertices(), _outputTopology))
					yield return primitive;
			}
		}

		private IEnumerable<VertexShaderOutput> GetVertices()
		{
			foreach (var executionResponse in VirtualMachine.ExecuteMultiple())
			{
				switch (executionResponse)
				{
					case ExecutionResponse.Finished:
						break;
					case ExecutionResponse.Emit:
						var outputs = GetShaderOutputs(0);
						yield return new VertexShaderOutput
						{
							Position = outputs[_outputPositionRegister],
							Data = outputs
						};
						break;
					case ExecutionResponse.Cut:
						yield return new VertexShaderOutput
						{
							IsStripCut = true
						};
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}