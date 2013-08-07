using SlimShader;

namespace Rasterizr.Resources
{
	public abstract class TextureBase : Resource
	{
		internal abstract class TextureSubresource : ISubresource
		{
            private readonly Number4[] _data;

            public Number4[] Data
			{
				get { return _data; }
			}

			protected TextureSubresource(int numElements)
			{
                _data = new Number4[numElements];
			}

            public void Clear(ref Number4 value)
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

        public abstract Number4[] GetData(int subresource);
        public abstract void SetData(int subresource, Number4[] data);
	}
}