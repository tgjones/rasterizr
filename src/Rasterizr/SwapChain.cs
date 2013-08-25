using Rasterizr.Resources;

namespace Rasterizr
{
	public class SwapChain : DeviceChild
	{
		private readonly SwapChainDescription _description;
	    private readonly ISwapChainPresenter _presenter;
	    private readonly Texture2D _backBuffer;

		public SwapChainDescription Description
		{
			get { return _description; }
		}

		internal SwapChain(Device device, SwapChainDescription description, ISwapChainPresenter presenter)
            : base(device)
		{
			_description = description;

            presenter.Initialize(description.Width, description.Height);
		    _presenter = presenter;

		    _backBuffer = new Texture2D(device, new Texture2DDescription
			{
				Width = description.Width,
				Height = description.Height,
				MipLevels = 1,
				ArraySize = 1
			});
		}

		public Texture2D GetBuffer(int index)
		{
			return _backBuffer;
		}

		internal void Present()
		{
			// TODO: Resolve multi-sampled back buffer.
			var resolvedColors = _backBuffer.GetData(0, 0);
            _presenter.Present(resolvedColors);
		}
	}
}