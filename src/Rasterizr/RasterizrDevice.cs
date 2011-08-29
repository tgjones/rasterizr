using Nexus;

namespace Rasterizr
{
	public class RasterizrDevice
	{
		#region Properties

		public RenderPipeline RenderPipeline { get; private set; }

		#endregion

		#region Constructor

		public RasterizrDevice(int width, int height)
		{
			RenderPipeline = new RenderPipeline();
			RenderPipeline.Rasterizer.Viewport = new Viewport3D
			{
				X = 0, 
				Y = 0, 
				Width = width,
				Height = height
			};
		}

		#endregion

		public void ClearDepthBuffer(float depth)
		{
			RenderPipeline.OutputMerger.DepthBuffer.Clear(depth);
		}

		public void ClearRenderTarget(ColorF color)
		{
			RenderPipeline.OutputMerger.RenderTarget.Clear(color);
		}

		public void Draw()
		{
			RenderPipeline.Draw();
		}
	}
}