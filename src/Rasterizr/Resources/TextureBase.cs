using Rasterizr.Util;

namespace Rasterizr.Resources
{
	public abstract class TextureBase : Resource
	{
		internal abstract class TextureSubresource : ISubresource
		{
			private readonly byte[] _data;
			private readonly int _elementSize;

			public byte[] Data
			{
				get { return _data; }
			}

			public int ElementSize
			{
				get { return _elementSize; }
			}

			protected TextureSubresource(int sizeInBytes, int elementSize)
			{
				_data = new byte[sizeInBytes];
				_elementSize = elementSize;
			}

			public void Clear<T>(ref T value)
				where T : struct
			{
				Utilities.Clear(_data, _elementSize, ref value);
			}
		}

		internal static void CalculateArrayMipSlice(int subresource, int mipLevels, out int mipSlice, out int arraySlice)
		{
			mipSlice = subresource % mipLevels;
			arraySlice = (subresource - mipSlice) / mipLevels;
		}

		private readonly Format _format;

		internal Format Format
		{
			get { return _format; }
		}

		protected TextureBase(Device device, Format format)
			: base(device)
		{
			_format = format;
		}
	}
}