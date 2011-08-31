using System.Collections.Concurrent;
using System.Threading.Tasks;
using Nexus;
using Rasterizr.InputAssembler;
using Rasterizr.OutputMerger;
using Rasterizr.Rasterizer;
using Rasterizr.ShaderStages.GeometryShader;
using Rasterizr.ShaderStages.PixelShader;
using Rasterizr.ShaderStages.VertexShader;

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
		public OutputMergerStage OutputMerger { get; set; }

		#endregion

		#region Constructor

		public RasterizrDevice(int width, int height)
		{
			InputAssembler = new InputAssemblerStage();
			VertexShader = new VertexShaderStage();
			GeometryShader = new GeometryShaderStage();

			PixelShader = new PixelShaderStage();
			OutputMerger = new OutputMergerStage();

			Rasterizer = new RasterizerStage(PixelShader, OutputMerger)
			{
				Viewport = new Viewport3D
				{
					X = 0,
					Y = 0,
					Width = width,
					Height = height
				}
			};

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

			var taskFactory = Task.Factory;
			Task.WaitAll(
				taskFactory.StartNew(() => InputAssembler.Run(inputAssemblerOutputs)),
				taskFactory.StartNew(() => VertexShader.Run(inputAssemblerOutputs, vertexShaderOutputs)),
				taskFactory.StartNew(() => GeometryShader.Run(vertexShaderOutputs, geometryShaderOutputs)),
				taskFactory.StartNew(() => Rasterizer.Run(geometryShaderOutputs, rasterizerOutputs)),
				taskFactory.StartNew(() => PixelShader.Run(rasterizerOutputs, pixelShaderOutputs)),
				taskFactory.StartNew(() => OutputMerger.Run(pixelShaderOutputs)));
		}
	}
}