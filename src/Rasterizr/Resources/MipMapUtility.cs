using System;
using System.Linq;
using Rasterizr.Util;

namespace Rasterizr.Resources
{
	internal static class MipMapUtility
	{
		public static int CalculateMipMapCount(int mipLevels, params int[] dimensions)
		{
			if (mipLevels != 1 && dimensions.Any(x => !Utilities.IsPowerOfTwo(x)))
				throw new ArgumentException("MipLevels must be 1 for non-power of 2 dimensions.");

			if (mipLevels != 0)
				return mipLevels;

			return (int) System.Math.Log(dimensions.Max(), 2.0) + 1;
		}

		public static Texture1D.Texture1DSubresource[] CreateMipMaps(int mipLevels, int elementSize, int width)
		{
			var result = new Texture1D.Texture1DSubresource[mipLevels];
			for (int level = 0; level < mipLevels; level++)
			{
				result[level] = new Texture1D.Texture1DSubresource(elementSize, width);

				width = System.Math.Max(width / 2, 1);
			}
			return result;
		}

		public static Texture2D.Texture2DSubresource[] CreateMipMaps(int mipLevels, int elementSize, int width, int height)
		{
			var result = new Texture2D.Texture2DSubresource[mipLevels];
			for (int level = 0; level < mipLevels; level++)
			{
				result[level] = new Texture2D.Texture2DSubresource(elementSize, width, height);

				width = System.Math.Max(width / 2, 1);
				height = System.Math.Max(height / 2, 1);
			}
			return result;
		}

		public static Texture3D.Texture3DSubresource[] CreateMipMaps(int mipLevels, int elementSize, int width, int height, int depth)
		{
			var result = new Texture3D.Texture3DSubresource[mipLevels];
			for (int level = 0; level < mipLevels; level++)
			{
				result[level] = new Texture3D.Texture3DSubresource(elementSize, width, height, depth);

				width = System.Math.Max(width / 2, 1);
				height = System.Math.Max(height / 2, 1);
				depth = System.Math.Max(depth / 2, 1);
			}
			return result;
		}
	}
}