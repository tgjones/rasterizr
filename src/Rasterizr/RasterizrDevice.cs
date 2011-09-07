using System.Collections.Concurrent;
using System.Threading.Tasks;
using Nexus;
using Rasterizr.InputAssembler;
using Rasterizr.OutputMerger;
using Rasterizr.Rasterizer;
using Rasterizr.ShaderStages.GeometryShader;
using Rasterizr.ShaderStages.PixelShader;
using Rasterizr.ShaderStages.VertexShader;
using System;

namespace Rasterizr
{
	public class RasterizrDevice
	{
		#region Properties

		public InputAssemblerStage InputAssembler { get; private set; }
		public VertexShaderStage VertexShader { get; private set; }
		public GeometryShaderStage GeometryShader { get; private set; }
		public RasterizerStage Rasterizer { get; private set; }
		public PixelShaderStage PixelShader { get; private set; }
		public OutputMergerStage OutputMerger { get; private set; }

		#endregion

		#region Constructor

		public RasterizrDevice()
		{
			InputAssembler = new InputAssemblerStage();
			VertexShader = new VertexShaderStage();
			GeometryShader = new GeometryShaderStage();

			PixelShader = new PixelShaderStage();
			OutputMerger = new OutputMergerStage();

			Rasterizer = new RasterizerStage(PixelShader, OutputMerger);
		}

		#endregion

		public void ClearDepthBuffer(float depth)
		{
			OutputMerger.DepthBuffer.Clear(depth);
		}

		public void ClearRenderTarget(ColorF color)
		{
			OutputMerger.RenderTarget.Clear(color);
		}

		public void Draw()
		{
			var inputAssemblerOutputs = new BlockingCollection<object>();
			var vertexShaderOutputs = new BlockingCollection<IVertexShaderOutput>();
			var geometryShaderOutputs = new BlockingCollection<IVertexShaderOutput>();
			var rasterizerOutputs = new BlockingCollection<Fragment>();
			var pixelShaderOutputs = new BlockingCollection<Pixel>();

			InputAssembler.Validate();

			var taskFactory = Task.Factory;
			try
			{
				Task.WaitAll(
					taskFactory.StartNew(() => InputAssembler.Run(inputAssemblerOutputs)),
					taskFactory.StartNew(() => VertexShader.Run(inputAssemblerOutputs, vertexShaderOutputs)),
					taskFactory.StartNew(() => GeometryShader.Run(vertexShaderOutputs, geometryShaderOutputs)),
					taskFactory.StartNew(() => Rasterizer.Run(geometryShaderOutputs, rasterizerOutputs)),
					taskFactory.StartNew(() => PixelShader.Run(rasterizerOutputs, pixelShaderOutputs)),
					taskFactory.StartNew(() => OutputMerger.Run(pixelShaderOutputs)));
			}
			catch (AggregateException ex)
			{
				throw new Exception(ex.InnerException.Message, ex);
			}
		}
	}
}