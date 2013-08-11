using System;
using System.Linq;
using Rasterizr.Math;
using Rasterizr.Util;

namespace Rasterizr.Resources
{
	internal static class MipMapUtility
	{
		public static int CalculateMipMapCount(int mipLevels, params int[] dimensions)
		{
			if (mipLevels != 1 && dimensions.Any(x => !MathUtility.IsPowerOfTwo(x)))
				throw new ArgumentException("MipLevels must be 1 for non-power of 2 dimensions.");

			if (mipLevels != 0)
				return mipLevels;

			return (int) MathUtility.Log2(dimensions.Max()) + 1;
		}

		public static Texture1D.Texture1DSubresource[] CreateMipMaps(int mipLevels, int width)
		{
			var result = new Texture1D.Texture1DSubresource[mipLevels];
			for (int level = 0; level < mipLevels; level++)
			{
				result[level] = new Texture1D.Texture1DSubresource(width);

				width = System.Math.Max(width / 2, 1);
			}
			return result;
		}

		public static Texture2D.Texture2DSubresource[] CreateMipMaps(int mipLevels, int width, int height)
		{
			var result = new Texture2D.Texture2DSubresource[mipLevels];
			for (int level = 0; level < mipLevels; level++)
			{
				result[level] = new Texture2D.Texture2DSubresource(width, height);

				width = System.Math.Max(width / 2, 1);
				height = System.Math.Max(height / 2, 1);
			}
			return result;
		}

		public static Texture3D.Texture3DSubresource[] CreateMipMaps(int mipLevels, int width, int height, int depth)
		{
			var result = new Texture3D.Texture3DSubresource[mipLevels];
			for (int level = 0; level < mipLevels; level++)
			{
				result[level] = new Texture3D.Texture3DSubresource(width, height, depth);

				width = System.Math.Max(width / 2, 1);
				height = System.Math.Max(height / 2, 1);
				depth = System.Math.Max(depth / 2, 1);
			}
			return result;
		}
	}
}