using System;
using Rasterizr.Util;

namespace Rasterizr.Resources
{
	public class Texture2D : TextureBase
	{
		internal class Texture2DSubresource : TextureSubresource
		{
			public int Width { get; private set; }
			public int Height { get; private set; }

			public Texture2DSubresource(int elementSize, int width, int height)
				: base(width * height * elementSize, elementSize)
			{
				Width = width;
				Height = height;
			}

			public int CalculateByteOffset(int x, int y)
			{
				return ((y * Width) + x) * ElementSize;
			}
		}

		private readonly Texture2DDescription _description;
		private readonly Texture2DSubresource[][] _subresources;

		public static Texture2D FromSwapChain<T>(SwapChain swapChain, int index)
		{
			return swapChain.GetBuffer<T>(index);
		}

		public Texture2DDescription Description
		{
			get { return _description; }
		}

		internal override ResourceType ResourceType
		{
			get { return ResourceType.Texture2D; }
		}

		internal Texture2D(Device device, Texture2DDescription description)
			: base(device, description.Format)
		{
			if (description.ArraySize == 0)
				throw new ArgumentException("ArraySize must be at least 1.");

			_description = description;

			_subresources = new Texture2DSubresource[description.ArraySize][];
			int mipMapCount = MipMapUtility.CalculateMipMapCount(description.MipLevels,
				description.Width, description.Height);
			for (int i = 0; i < description.ArraySize; i++)
				_subresources[i] = MipMapUtility.CreateMipMaps(mipMapCount, 
					FormatHelper.SizeOfInBytes(description.Format),
					description.Width, description.Height);
		}

		internal void GetData<T>(int arrayIndex, int mipSlice, T[] data)
			where T : struct
		{
			var source = _subresources[arrayIndex][mipSlice].Data;
			Utilities.FromByteArray(data, 0, source, 0, source.Length);
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
	}
}