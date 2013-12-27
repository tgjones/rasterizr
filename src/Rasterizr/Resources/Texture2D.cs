using System;
using System.Linq;
using Rasterizr.Util;
using SlimShader;
using SlimShader.VirtualMachine.Resources;

namespace Rasterizr.Resources
{
	public class Texture2D : TextureBase
	{
		internal class Texture2DSubresource : TextureSubresource, ITextureMipMap
		{
			public int Width { get; private set; }
			public int Height { get; private set; }

		    public int Depth
		    {
		        get { return 0; }
		    }

		    public Texture2DSubresource(int width, int height)
				: base(width * height)
			{
				Width = width;
				Height = height;
			}

            Number4 ITextureMipMap.GetData(ref Number4 coords)
            {
                return Data[((coords.Int1 * Width) + coords.Int0)];
            }

            public Number4 GetData(int x, int y)
			{
				return Data[((y * Width) + x)];
			}

            public void SetData(int x, int y, ref Number4 value)
            {
                Data[((y * Width) + x)] = value;
            }
		}

		private readonly Texture2DDescription _description;
		private readonly Texture2DSubresource[][] _subresources;

		public static Texture2D FromSwapChain(SwapChain swapChain, int index)
		{
			return swapChain.GetBuffer(index);
		}

		public Texture2DDescription Description
		{
			get { return _description; }
		}

		internal override ResourceType ResourceType
		{
			get { return ResourceType.Texture2D; }
		}

        internal override int Size
        {
            get { return _subresources.Sum(x => x.Sum(y => y.Size)); }
        }

		internal Texture2D(Device device, Texture2DDescription description)
			: base(device)
		{
			if (description.ArraySize == 0)
				throw new ArgumentException("ArraySize must be at least 1.");

			int mipMapCount = MipMapUtility.CalculateMipMapCount(description.MipLevels,
				description.Width, description.Height);

			_description = description;
			_description.MipLevels = mipMapCount;

			_subresources = new Texture2DSubresource[description.ArraySize][];
			
			for (int i = 0; i < description.ArraySize; i++)
				_subresources[i] = MipMapUtility.CreateMipMaps(mipMapCount, 
					description.Width, description.Height);
		}

        public override Color4[] GetData(int subresource)
        {
            int mipSlice, arrayIndex;
            CalculateArrayMipSlice(subresource, _subresources[0].Length, out mipSlice, out arrayIndex);

            var data = _subresources[arrayIndex][mipSlice].Data;
            var result = new Color4[data.Length];
            Utilities.Copy(data, result);
            return result;
        }

        internal override void SetData(int subresource, Color4[] data)
		{
			int mipSlice, arrayIndex;
			CalculateArrayMipSlice(subresource, _subresources[0].Length, out mipSlice, out arrayIndex);

            Utilities.Copy(data, _subresources[arrayIndex][mipSlice].Data);
		}

        internal Number4[] GetData(int arrayIndex, int mipSlice)
		{
			return _subresources[arrayIndex][mipSlice].Data;
		}

		internal Texture2DSubresource GetSubresource(int arrayIndex, int mipSlice)
		{
			return _subresources[arrayIndex][mipSlice];
		}

		internal void GetDimensions(int mipLevel, out int width, out int height, out int numberOfLevels)
		{
			GetDimensions(mipLevel, out width, out height);
			numberOfLevels = _subresources[0].Length;
		}

		internal void GetDimensions(int mipLevel, out int width, out int height)
		{
			var subresource = _subresources[0][mipLevel];
			width = subresource.Width;
			height = subresource.Height;
		}

		internal void GetDimensions(out int width, out int height)
		{
			GetDimensions(0, out width, out height);
		}

		internal override void GenerateMips()
		{
			for (int i = 0; i < _description.ArraySize; i++)
				for (int mipSlice = 1; mipSlice < _subresources[0].Length; ++mipSlice)
				{
					var subresource = GetSubresource(i, mipSlice);
					int width = subresource.Width;
					int height = subresource.Height;
					for (int y = 0; y < height; ++y)
					{
						for (int x = 0; x < width; ++x)
						{
							int previousLevelX = x * 2;
							int previousLevelY = y * 2;

							var moreDetailedMipLevel = GetSubresource(i, mipSlice - 1);
							var c00 = moreDetailedMipLevel.GetData(previousLevelX, previousLevelY);
							var c10 = moreDetailedMipLevel.GetData(ClampToDimension(previousLevelX + 1, moreDetailedMipLevel.Width), previousLevelY);
                            var c01 = moreDetailedMipLevel.GetData(previousLevelX, ClampToDimension(previousLevelY + 1, moreDetailedMipLevel.Height));
                            var c11 = moreDetailedMipLevel.GetData(ClampToDimension(previousLevelX + 1, moreDetailedMipLevel.Width), ClampToDimension(previousLevelY + 1, moreDetailedMipLevel.Height));
							var interpolatedColor = Number4.Average(ref c00, ref c10, ref c01, ref c11);

						    subresource.SetData(x, y, ref interpolatedColor);
						}
					}
				}
		}

        /// <summary>
        /// Takes care of non-power-of-two textures not necessarily having double the size in all dimensions.
        /// </summary>
        private static int ClampToDimension(int value, int dimension)
	    {
	        return System.Math.Min(value, dimension - 1);
	    }

        public static Texture2D FromColors(Device device, Color4[] data, int width, int height)
        {
            var result = device.CreateTexture2D(new Texture2DDescription
            {
                Width = width,
                Height = height,
                MipLevels = 0,
                ArraySize = 1,
            });

            device.ImmediateContext.SetTextureData(result, 0, data);
            device.ImmediateContext.GenerateMips(result);

            return result;
        }
	}
}