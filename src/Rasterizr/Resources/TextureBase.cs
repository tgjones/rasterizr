using Rasterizr.Math;

namespace Rasterizr.Resources
{
	public abstract class TextureBase : Resource
	{
		internal abstract class TextureSubresource : ISubresource
		{
			private readonly Color4F[] _data;

			public Color4F[] Data
			{
				get { return _data; }
			}

			protected TextureSubresource(int numElements)
			{
                _data = new Color4F[numElements];
			}

		    public void Clear(ref Color4F value)
		    {
		        for (var i = 0; i < _data.Length; i++)
		            _data[i] = value;
		    }
		}

		internal static void CalculateArrayMipSlice(int subresource, int mipLevels, out int mipSlice, out int arraySlice)
		{
			mipSlice = subresource % mipLevels;
			arraySlice = (subresource - mipSlice) / mipLevels;
		}

		protected TextureBase(Device device)
			: base(device)
		{
			
		}

	    public abstract Color4F[] GetData(int subresource);
	    public abstract void SetData(int subresource, Color4F[] data);
	}
}