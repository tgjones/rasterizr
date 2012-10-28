using System.Collections.Generic;
using Nexus.Graphics.Colors;
using Rasterizr.Diagnostics;
using Rasterizr.InputAssembler;
using Rasterizr.OutputMerger;
using Rasterizr.Rasterizer;
using Rasterizr.ShaderCore;
using Rasterizr.ShaderCore.GeometryShader;
using Rasterizr.ShaderCore.PixelShader;
using Rasterizr.ShaderCore.VertexShader;

namespace Rasterizr
{
	public class RasterizrDevice
	{
		private readonly GraphicsLoggerCollection _loggers;

		#region Properties

		internal GraphicsLoggerCollection Loggers
		{
			get { return _loggers; }
		}

		public InputAssemblerStage InputAssembler { get; private set; }
		public VertexShaderStage VertexShader { get; private set; }
		public GeometryShaderStage GeometryShader { get; private set; }
		public RasterizerStage Rasterizer { get; private set; }
		public PixelShaderStage PixelShader { get; private set; }
		public OutputMergerStage OutputMerger { get; private set; }

		#endregion

		#region Constructor

		public RasterizrDevice(IList<GraphicsLogger> loggers = null)
		{
			_loggers = new GraphicsLoggerCollection(loggers ?? new List<GraphicsLogger>());

			_loggers.BeginOperation(OperationType.CreateDevice);

			InputAssembler = new InputAssemblerStage(this);
			VertexShader = new VertexShaderStage(InputAssembler);
			GeometryShader = new GeometryShaderStage();

			PixelShader = new PixelShaderStage();
			OutputMerger = new OutputMergerStage();

			Rasterizer = new RasterizerStage(VertexShader, PixelShader, OutputMerger);
		}

		#endregion

		public void ClearDepthBuffer(float depth)
		{
			_loggers.BeginOperation(OperationType.DeviceClearDepthBuffer, depth);
			OutputMerger.DepthBuffer.Clear(depth);
		}

		public void ClearRenderTarget(ColorF color)
		{
			_loggers.BeginOperation(OperationType.DeviceClearRenderTarget, color);
			OutputMerger.RenderTarget.Clear(color);
		}

		public void Draw(int vertexCount, int startVertexLocation)
		{
			_loggers.BeginOperation(OperationType.DeviceDraw, vertexCount, startVertexLocation);
			BeginDraw();
			var inputAssemblerOutputs = InputAssembler.Run(vertexCount, startVertexLocation);
			DrawInternal(inputAssemblerOutputs);
		}

		public void DrawIndexed(int indexCount, int startIndexLocation, int baseVertexLocation)
		{
			_loggers.BeginOperation(OperationType.DeviceDrawIndexed, indexCount, startIndexLocation, baseVertexLocation);
			BeginDraw();
			var inputAssemblerOutputs = InputAssembler.RunIndexed(indexCount, startIndexLocation, baseVertexLocation);
			DrawInternal(inputAssemblerOutputs);
		}

		private void BeginDraw()
		{
			new ShaderValidator().CheckCompatibility(VertexShader.VertexShader, PixelShader.PixelShader);
		}

		private void DrawInternal(IEnumerable<InputAssemblerOutput> inputAssemblerOutputs)
		{
			var vertexShaderOutputs = VertexShader.Run(inputAssemblerOutputs);
			var geometryShaderOutputs = GeometryShader.Run(vertexShaderOutputs);
			var rasterizerOutputs = Rasterizer.Run(geometryShaderOutputs);
			var pixelShaderOutputs = PixelShader.Run(rasterizerOutputs);
			OutputMerger.Run(pixelShaderOutputs);
		}
	}
}