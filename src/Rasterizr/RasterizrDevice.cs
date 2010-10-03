namespace Rasterizr
{
	public class RasterizrDevice
	{
		#region Properties

		public RenderPipeline RenderPipeline { get; private set; }
		public Viewport3D Viewport { get; set; }

		#endregion

		#region Constructor

		public RasterizrDevice(int width, int height)
		{
			Viewport = new Viewport3D { X = 0, Y = 0, Width = width, Height = height };
			RenderPipeline = new RenderPipeline(Viewport);
		}

		#endregion

		public void BeginFrame()
		{
			RenderPipeline.Clear();
		}

		public void Draw()
		{
			RenderPipeline.Draw();
		}

		public void Present()
		{
			RenderPipeline.Present();
		}
	}
}