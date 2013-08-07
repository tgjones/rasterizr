using System.Diagnostics;
using Rasterizr.Diagnostics;
using Rasterizr.Resources;
using SlimShader;

namespace Rasterizr
{
	public abstract class SwapChain
	{
		private readonly Device _device;
		private readonly SwapChainDescription _description;
		private readonly Texture2D _backBuffer;

		public SwapChainDescription Description
		{
			get { return _description; }
		}

		protected SwapChain(Device device, SwapChainDescription description)
		{
			device.Loggers.BeginOperation(OperationType.SwapChainCreate, description);
			_device = device;
			_description = description;
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

		private int _frameCounter;
		public void Present()
		{
			_device.Loggers.BeginOperation(OperationType.SwapChainPresent);

			// TODO: Resolve multi-sampled back buffer.
			var resolvedColors = _backBuffer.GetData(0, 0);
			Present(resolvedColors);

			Debug.WriteLine("Done frame " + _frameCounter++);
		}

        protected abstract void Present(Number4[] colors);
	}
}