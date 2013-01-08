using System;
using Rasterizr.Math;
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

			int mipMapCount = MipMapUtility.CalculateMipMapCount(description.MipLevels,
				description.Width, description.Height);

			_description = description;
			_description.MipLevels = mipMapCount;

			_subresources = new Texture2DSubresource[description.ArraySize][];
			
			for (int i = 0; i < description.ArraySize; i++)
				_subresources[i] = MipMapUtility.CreateMipMaps(mipMapCount, 
					FormatHelper.SizeOfInBytes(description.Format),
					description.Width, description.Height);
		}

		internal override MappedSubresource Map(int subresource)
		{
			int mipSlice, arrayIndex;
			CalculateArrayMipSlice(subresource, _subresources[0].Length, out mipSlice, out arrayIndex);

			return new MappedSubresource
			{
				Data = _subresources[arrayIndex][mipSlice].Data
			};
		}

		internal override void UpdateSubresource(int subresource, byte[] data)
		{
			int mipSlice, arrayIndex;
			CalculateArrayMipSlice(subresource, _subresources[0].Length, out mipSlice, out arrayIndex);

			Array.Copy(data, _subresources[arrayIndex][mipSlice].Data, data.Length);
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

		internal void GenerateMips()
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
							Color4F c00 = GetColor(moreDetailedMipLevel, previousLevelX, previousLevelY);
							Color4F c10 = GetColor(moreDetailedMipLevel, previousLevelX + 1, previousLevelY);
							Color4F c01 = GetColor(moreDetailedMipLevel, previousLevelX, previousLevelY + 1);
							Color4F c11 = GetColor(moreDetailedMipLevel, previousLevelX + 1, previousLevelY + 1);
							Color4F interpolatedColor = (c00 + c10 + c01 + c11) / 4.0f;

							FormatHelper.Convert(Format, interpolatedColor, new DataIndex
							{
								Data = subresource.Data,
								Offset = subresource.CalculateByteOffset(x, y)
							});
						}
					}
				}
		}

		private Color4F GetColor(Texture2DSubresource subresource, int x, int y)
		{
			return FormatHelper.Convert(Format, new DataIndex
			{
				Data = subresource.Data,
				Offset = subresource.CalculateByteOffset(x, y)
			});
		}
	}
}