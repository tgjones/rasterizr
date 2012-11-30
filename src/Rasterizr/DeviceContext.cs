using System.Collections.Generic;
using Rasterizr.Math;
using Rasterizr.Pipeline;
using Rasterizr.Pipeline.GeometryShader;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Pipeline.PixelShader;
using Rasterizr.Pipeline.Rasterizer;
using Rasterizr.Pipeline.VertexShader;
using Rasterizr.Resources;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr
{
	public class DeviceContext
	{
		private readonly InputAssemblerStage _inputAssembler;
		private readonly VertexShaderStage _vertexShader;
		private readonly GeometryShaderStage _geometryShader;
		private readonly RasterizerStage _rasterizer;
		private readonly PixelShaderStage _pixelShader;
		private readonly OutputMergerStage _outputMerger;

		public InputAssemblerStage InputAssembler
		{
			get { return _inputAssembler; }
		}

		public VertexShaderStage VertexShader
		{
			get { return _vertexShader; }
		}

		public GeometryShaderStage GeometryShader
		{
			get { return _geometryShader; }
		}

		public RasterizerStage Rasterizer
		{
			get { return _rasterizer; }
		}

		public PixelShaderStage PixelShader
		{
			get { return _pixelShader; }
		}

		public OutputMergerStage OutputMerger
		{
			get { return _outputMerger; }
		}

		public DeviceContext(Device device)
		{
			_inputAssembler = new InputAssemblerStage();
			_vertexShader = new VertexShaderStage();
			_geometryShader = new GeometryShaderStage();
			_rasterizer = new RasterizerStage(device);
			_pixelShader = new PixelShaderStage();
			_outputMerger = new OutputMergerStage(device);
		}

		public void ClearDepthStencilView(DepthStencilView depthStencilView, DepthStencilClearFlags clearFlags, float depth, byte stencil)
		{
			depthStencilView.Clear(clearFlags, depth, stencil);
		}

		public void ClearRenderTargetView(RenderTargetView renderTargetView, Color4F color)
		{
			renderTargetView.Clear(color);
		}

		public void Draw(int vertexCount, int startVertexLocation)
		{
			DrawInternal(_inputAssembler.GetVertexStream(vertexCount, startVertexLocation));
		}

		public void DrawIndexed(int indexCount, int startIndexLocation, int baseVertexLocation)
		{
			DrawInternal(_inputAssembler.GetVertexStreamIndexed(indexCount, startIndexLocation, baseVertexLocation));
		}

		public void DrawInstanced(int vertexCountPerInstance, int instanceCount, int startVertexLocation, int startInstanceLocation)
		{
			DrawInternal(_inputAssembler.GetVertexStreamInstanced(vertexCountPerInstance,
				instanceCount, startVertexLocation, startInstanceLocation));
		}

		private void DrawInternal(IEnumerable<InputAssemblerVertexOutput> vertexStream)
		{
			var vertexShaderOutputs = _vertexShader.Execute(vertexStream);
			var primitiveStream = _inputAssembler.GetPrimitiveStream(vertexShaderOutputs);

			IEnumerable<InputAssemblerPrimitiveOutput> rasterizerInputs;
			PrimitiveTopology rasterizerInputTopology;
			OutputSignatureChunk rasterizerInputSignature;
			if (_geometryShader.IsActive)
			{
				rasterizerInputs = _geometryShader.Execute(primitiveStream, _inputAssembler.PrimitiveTopology);
				rasterizerInputTopology = _geometryShader.OutputTopology;
				rasterizerInputSignature = _geometryShader.Shader.Bytecode.OutputSignature;
			}
			else
			{
				rasterizerInputs = primitiveStream;
				rasterizerInputTopology = _inputAssembler.PrimitiveTopology;
				rasterizerInputSignature = _vertexShader.Shader.Bytecode.OutputSignature;
			}

			var rasterizerOutputs = _rasterizer.Execute(rasterizerInputs, rasterizerInputTopology,
				rasterizerInputSignature, _pixelShader.Shader.Bytecode.InputSignature,
				_outputMerger.MultiSampleCount);

			var pixelShaderOutputs = _pixelShader.Execute(rasterizerOutputs);
			_outputMerger.Execute(pixelShaderOutputs);
		}
	}
}