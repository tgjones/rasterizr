using SlimShader;

namespace Rasterizr
{
	public class RawSwapChain : SwapChain
	{
        private Number4[] _data;

        public Number4[] Data
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

        protected override void Present(Number4[] colors)
		{
			_data = colors;
		}
	}
}