using Rasterizr.Math;

namespace Rasterizr
{
	public class RawSwapChain : SwapChain
	{
		private Color4F[] _data;

		public Color4F[] Data
		{
			get { return _data; }
		}

		public RawSwapChain(Device device, int width, int height)
			: base(device, new SwapChainDescription
			{
				Width = width,
				Height = height,
				SampleDescription = new SampleDescription(1)
			})
		{
		
		}

		protected override void Present(Color4F[] colors)
		{
			_data = colors;
		}
	}
}