using Nexus.Graphics;

namespace Rasterizr.OutputMerger
{
	public class DepthBufferView
	{
		private readonly Surface<float> _surface;

		public float this[int x, int y, int sampleIndex]
		{
			get { return _surface[x, y, sampleIndex]; }
			set { _surface[x, y, sampleIndex] = value; }
		}

		public DepthBufferView(Surface<float> surface)
		{
			_surface = surface;
		}

		public void Clear(float value)
		{
			_surface.Clear(value);
		}
	}
}