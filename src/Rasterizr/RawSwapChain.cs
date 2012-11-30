namespace Rasterizr
{
	public class RawSwapChain : SwapChain
	{
		private byte[] _data;

		public byte[] Data
		{
			get { return _data; }
		}

		public RawSwapChain(Device device, int width, int height)
			: base(device, new SwapChainDescription
			{
				Width = width,
				Height = height,
				Format = Format.R8G8B8A8_UInt,
				SampleDescription = new SampleDescription(1)
			})
		{
		
		}

		protected override void Present(byte[] colors)
		{
			_data = colors;
		}
	}
}