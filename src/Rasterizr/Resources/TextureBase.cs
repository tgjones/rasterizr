using Rasterizr.Util;
using SlimShader;

namespace Rasterizr.Resources
{
	public abstract class TextureBase : Resource
	{
		internal abstract class TextureSubresource : ISubresource
		{
            internal readonly Number4[] Data;

		    internal int Size
		    {
		        get { return Data.Length * Utilities.SizeOf<Number4>(); }
		    }

			protected TextureSubresource(int numElements)
			{
                Data = new Number4[numElements];
			}

            public void Clear(ref Number4 value)
		    {
		        for (var i = 0; i < Data.Length; i++)
		            Data[i] = value;
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

        public abstract Color4[] GetData(int subresource);

        internal abstract void SetData(int subresource, Color4[] data);
	    internal abstract void GenerateMips();
	}
}