using System.Diagnostics;
using Rasterizr.Resources;

namespace Rasterizr
{
	public abstract class SwapChain
	{
		private readonly SwapChainDescription _description;
		private readonly Resource _backBuffer;
		private readonly byte[] _resolvedColors;

		public SwapChainDescription Description
		{
			get { return _description; }
		}

		protected SwapChain(Device device, SwapChainDescription description)
		{
			_description = description;
			_backBuffer = new Texture2D(device, new Texture2DDescription
			{
				Width = description.Width,
				Height = description.Height,
				Format = description.Format
			});
			_resolvedColors = new byte[description.Width * description.Height * FormatHelper.SizeOfInBytes(description.Format)];
		}

		public T GetBuffer<T>(int index)
			where T : Resource
		{
			return (T) _backBuffer;
		}

		private int _frameCounter;
		public void Present()
		{
			// TODO: Resolve multi-sampled back buffer.
			_backBuffer.GetData(_resolvedColors);
			Present(_resolvedColors);

			Debug.WriteLine("Done frame " + _frameCounter++);
		}

		protected abstract void Present(byte[] colors);
	}
}