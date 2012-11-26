using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Pipeline.PixelShader;
using Rasterizr.Pipeline.Rasterizer;
using Rasterizr.Pipeline.VertexShader;
using Rasterizr.Resources;

namespace Rasterizr
{
	public class DeviceContext
	{
		private readonly InputAssemblerStage _inputAssembler;
		private readonly VertexShaderStage _vertexShader;
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

		public DeviceContext()
		{
			_inputAssembler = new InputAssemblerStage();
			_vertexShader = new VertexShaderStage();
			_rasterizer = new RasterizerStage();
			_pixelShader = new PixelShaderStage();
			_outputMerger = new OutputMergerStage();
		}

		public void ClearDepthStencilView(DepthStencilView depthStencilView, DepthStencilClearFlags clearFlags, float depth, byte stencil)
		{
			
		}

		public void ClearRenderTargetView(RenderTargetView renderTargetView, Color4 color)
		{
			renderTargetView.Clear(color);
		}

		public void Draw(int vertexCount, int startVertexLocation)
		{
			
		}
	}
}