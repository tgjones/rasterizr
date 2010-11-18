using Nexus;
using Nexus.Graphics;

namespace Rasterizr.PipelineStages.OutputMerger
{
	public class RenderTargetView
	{
		private readonly ColorSurface _surface;

		public int Width
		{
			get { return _surface.Width; }
		}

		public int Height
		{
			get { return _surface.Height; }
		}

		public int MultiSampleCount
		{
			get { return _surface.MultiSampleCount; }
		}

		public ColorF this[int x, int y, int sampleIndex]
		{
			get { return _surface[x, y, sampleIndex]; }
			set { _surface[x, y, sampleIndex] = value; }
		}

		public RenderTargetView(ColorSurface surface)
		{
			_surface = surface;
		}

		public void Clear(ColorF value)
		{
			_surface.Clear(value);
		}
	}
}