using System.Windows.Media.Imaging;
using Nexus;
using Rasterizr.Util;

namespace Rasterizr.PipelineStages.OutputMerger
{
	public class WriteableBitmapRenderTarget : IRenderTarget
	{
		private readonly WriteableBitmapWrapper _wrapper;

		#region Properties

		public int Width
		{
			get { return _wrapper.Width; }
		}

		public int Height
		{
			get { return _wrapper.Height; }
		}

		#endregion

		public WriteableBitmapRenderTarget(WriteableBitmap bitmap)
		{
			_wrapper = new WriteableBitmapWrapper(bitmap);
		}

		public void Clear()
		{
			_wrapper.Clear(System.Windows.Media.Colors.LightGray);
		}

		public Color GetPixel(int x, int y)
		{
			System.Windows.Media.Color color = _wrapper.GetPixel(x, y);
			return new Color(color.A, color.R, color.G, color.B);
		}

		public void SetPixel(int x, int y, Color color)
		{
			_wrapper.SetPixel(x, y, System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
		}

		public void BeginFrame()
		{
			
		}

		public void EndFrame()
		{
			_wrapper.Invalidate();
		}
	}
}