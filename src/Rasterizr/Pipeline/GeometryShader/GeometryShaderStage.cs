using System;
using System.Collections.Generic;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.VertexShader;
using Rasterizr.Util;
using SlimShader;
using SlimShader.Chunks.Xsgn;
using SlimShader.VirtualMachine;

namespace Rasterizr.Pipeline.GeometryShader
{
	public class GeometryShaderStage : CommonShaderStage<GeometryShader>
	{
		private int _outputPositionRegister;
	    private int? _renderTargetArrayIndexRegister;
		private PrimitiveTopology _outputTopology;

		internal PrimitiveTopology OutputTopology
		{
			get { return _outputTopology; }
		}

		public bool IsActive
		{
			get { return Shader != null; }
		}

		public GeometryShaderStage(Device device)
			: base(device)
		{

		}

		protected override void OnShaderChanged(GeometryShader shader)
		{
		    if (shader != null)
		    {
		        var outputPositionRegister = GetSystemValueRegister(Name.Position);
		        if (outputPositionRegister == null)
		            throw new ArgumentException("Shader doesn't contain output position", "shader");
		        _outputPositionRegister = outputPositionRegister.Value;

                _renderTargetArrayIndexRegister = GetSystemValueRegister(Name.RenderTargetArrayIndex);

		        _outputTopology = shader.Bytecode.Statistics.GeometryShaderOutputTopology.ToPrimitiveTopology();
		    }
		    else
		    {
		        _outputPositionRegister = -1;
		        _renderTargetArrayIndexRegister = null;
		        _outputTopology = PrimitiveTopology.Undefined;
		    }

		    base.OnShaderChanged(shader);
		}

		internal IEnumerable<InputAssemblerPrimitiveOutput> Execute(IEnumerable<InputAssemblerPrimitiveOutput> inputs, PrimitiveTopology primitiveTopology)
		{
			SetShaderConstants();

			foreach (var input in inputs)
			{
				for (ushort i = 0; i < input.Vertices.Length; i++)
					SetShaderInputs(0, i, input.Vertices[i].OutputData);

			    foreach (var primitive in PrimitiveAssembler.GetPrimitiveStream(GetVertices(), _outputTopology))
			    {
			        if (_renderTargetArrayIndexRegister != null)
			        {
                        var tempPrimitive = primitive;
                        tempPrimitive.RenderTargetArrayIndex = primitive.Vertices[0]
                            .OutputData[_renderTargetArrayIndexRegister.Value]
                            .Number0.UInt;
			            yield return tempPrimitive;
			        }
			        else
			        {
                        yield return primitive;
			        }
			    }
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
                            InputData = new Number4[0],
							OutputData = outputs
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