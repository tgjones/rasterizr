using System;
using Nexus;
using Nexus.Graphics;

namespace Rasterizr.Core.ShaderCore
{
	// Filtering in this class is mainly thanks to "Essential Mathematics for Games & Interactive Applications"
	public class Texture2D
	{
		#region Static stuff

		public static Texture2D FromFile(string uri)
		{
			var surface = ColorSurfaceLoader.LoadFromFile(uri);
			var texture = new Texture2D(surface.Width, surface.Height, true);
			var data = new ColorF[surface.Width, surface.Height];
			for (int y = 0; y < surface.Height; y++)
				for (int x = 0; x < surface.Width; x++)
					data[x, y] = surface[x, y];
			texture.SetData(data);
			return texture;
		}

		#endregion

		private TextureMipMapLevel[] _levels;

		public Texture2D(int width, int height, bool mipMap)
		{
			CreateMipMaps(width, height, mipMap);
		}

		public void SetData(ColorF[,] data)
		{
			if (data.GetLength(0) != _levels[0].Width)
				throw new RasterizrException("Incorrect width");
			if (data.GetLength(1) != _levels[0].Height)
				throw new RasterizrException("Incorrect height");
			_levels[0].Texels = data;
			PopulateMipMaps();
		}

		#region Mipmaps

		private class TextureMipMapLevel
		{
			public ColorF[,] Texels { get; set; }

			public int Level { get; private set; }
			public int Width { get; private set; }
			public int Height { get; private set; }

			public TextureMipMapLevel(int level, int width, int height)
			{
				Level = level;
				Width = width;
				Height = height;

				Texels = new ColorF[width, height];
			}
		}

		private void CreateMipMaps(int width, int height, bool mipMap)
		{
			int numLevels = (mipMap) ? (int) MathUtility.Log2(Math.Max(width, height)) + 1 : 1;
			_levels = new TextureMipMapLevel[numLevels];

			for (int level = 0; level < numLevels; ++level)
			{
				_levels[level] = new TextureMipMapLevel(level, width, height);
				width /= 2;
				height /= 2;
			}
		}

		private void PopulateMipMaps()
		{
			for (int level = 1; level < _levels.Length; ++level)
			{
				int width = _levels[level].Width;
				int height = _levels[level].Height;
				for (int y = 0; y < height; ++y)
				{
					for (int x = 0; x < width; ++x)
					{
						int previousLevelX = x * 2;
						int previousLevelY = y * 2;
						ColorF c00 = GetColor(level - 1, previousLevelX, previousLevelY);
						ColorF c10 = GetColor(level - 1, previousLevelX + 1, previousLevelY);
						ColorF c01 = GetColor(level - 1, previousLevelX, previousLevelY + 1);
						ColorF c11 = GetColor(level - 1, previousLevelX + 1, previousLevelY + 1);
						ColorF interpolatedColor = (c00 + c10 + c01 + c11) / 4.0f;
						_levels[level].Texels[x, y] = interpolatedColor;
					}
				}
			}
		}

		#endregion
		
		public float CalculateLevelOfDetail(SamplerState samplerState)
		{
			Vector2D ddx, ddy;
			CalculatePartialDifferentials(out ddx, out ddy);
			return CalculateLevelOfDetail(ddx, ddy);
		}

		public void GetDimensions(int mipLevel, out int width, out int height, out int numberOfLevels)
		{
			GetDimensions(mipLevel, out width, out height);
			numberOfLevels = _levels.Length;
		}

		public void GetDimensions(int mipLevel, out int width, out int height)
		{
			width = _levels[mipLevel].Width;
			height = _levels[mipLevel].Height;
		}

		public void GetDimensions(out int width, out int height)
		{
			GetDimensions(0, out width, out height);
		}

		public ColorF Sample(SamplerState samplerState, Point2D location)
		{
			Vector2D ddx, ddy;
			CalculatePartialDifferentials(out ddx, out ddy);
			return SampleGrad(samplerState, location, ddx, ddy);
		}

		public ColorF SampleGrad(SamplerState samplerState, Point2D location, Vector2D ddx, Vector2D ddy)
		{
			return SampleLevel(samplerState, location, CalculateLevelOfDetail(ddx, ddy));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="samplerState"></param>
		/// <param name="location"></param>
		/// <param name="lod">A number that specifies the mipmap level. If the value is &lt;=0, the zero'th (biggest map)
		/// is used. The fractional value (if supplied) is used to interpolate between two mipmap levels.</param>
		/// <returns></returns>
		public ColorF SampleLevel(SamplerState samplerState, Point2D location, float lod)
		{
			switch (samplerState.Filter)
			{
				case TextureFilter.MinLinearMagMipPoint:
				case TextureFilter.MinMagLinearMipPoint:
				case TextureFilter.MinMagMipPoint:
					{
						// Calculate nearest mipmap level.
						int nearestLevel = MathUtility.Round(lod);
						return GetFilteredColor(samplerState.Filter, true, nearestLevel, samplerState, location.X, location.Y);
					}
				case TextureFilter.MinLinearMagPointMipLinear:
				case TextureFilter.MinMagMipLinear:
				case TextureFilter.MinMagPointMipLinear:
				case TextureFilter.MinPointMagMipLinear:
					{
						// Calculate nearest two levels and linearly filter between them.
						int nearestLevelInt = (int)lod;
						float d = lod - nearestLevelInt;
						ColorF c1 = GetFilteredColor(samplerState.Filter, true, nearestLevelInt, samplerState, location.X, location.Y);
						ColorF c2 = GetFilteredColor(samplerState.Filter, true, nearestLevelInt + 1, samplerState, location.X, location.Y);
						return (c1 * (1 - d)) + (c2 * d);
					}
				default:
					throw new NotSupportedException();
			}
		}

		private void CalculatePartialDifferentials(out Vector2D ddx, out Vector2D ddy)
		{
			throw new NotImplementedException();
		}

		private float CalculateLevelOfDetail(Vector2D ddx, Vector2D ddy)
		{
			int width, height;
			GetDimensions(out width, out height);
			float xBound2 = width * width;
			float yBound2 = height * height;

			float dudx2 = ddx.X * ddx.X * xBound2;
			float dvdx2 = ddx.Y * ddx.Y * yBound2;
			float dudy2 = ddy.X * ddy.X * xBound2;
			float dvdy2 = ddy.Y * ddy.Y * yBound2;

			// Proportional to the amount of a texel on display in a single pixel
			float pixelSizeTexelRatio2 = Math.Max(dudx2 + dvdx2, dudy2 + dvdy2);

			// Uses formula for p410 of Essential Mathematics for Games and Interactive Applications
			return 0.5f * MathUtility.Log2(pixelSizeTexelRatio2);
		}

		private ColorF GetColor(int level, int x, int y)
		{
			return _levels[level].Texels[x, y];
		}

		private ColorF GetFilteredColor(TextureFilter filter, bool minifying, int level, SamplerState samplerState, float u, float v)
		{
			level = MathUtility.Clamp(level, 0, _levels.Length - 1);

			int width, height;
			GetDimensions(level, out width, out height);
			u *= width;
			v *= height;

			// Minifying
			if (minifying)
				switch (filter)
				{
					case TextureFilter.MinMagMipPoint:
					case TextureFilter.MinMagPointMipLinear:
					case TextureFilter.MinPointMagMipLinear:
						return GetNearestNeighbor(samplerState, level, u, v);
					case TextureFilter.MinLinearMagMipPoint:
					case TextureFilter.MinLinearMagPointMipLinear:
					case TextureFilter.MinMagLinearMipPoint:
					case TextureFilter.MinMagMipLinear:
						return GetLinear(samplerState, level, u, v);
					default:
						throw new NotSupportedException();
				}

			// Magnifying
			switch (filter)
			{
				case TextureFilter.MinLinearMagMipPoint:
				case TextureFilter.MinLinearMagPointMipLinear:
				case TextureFilter.MinMagMipPoint:
				case TextureFilter.MinMagPointMipLinear:
					return GetNearestNeighbor(samplerState, level, u, v);
				case TextureFilter.MinMagLinearMipPoint:
				case TextureFilter.MinMagMipLinear:
				case TextureFilter.MinPointMagMipLinear:
					return GetLinear(samplerState, level, u, v);
				default:
					throw new NotSupportedException();
			}
		}

		private ColorF GetColor(SamplerState samplerState, int level, int texU, int texV)
		{
			int width, height;
			GetDimensions(level, out width, out height);
			int? modifiedTexelU = GetTextureAddress(texU, width, samplerState.AddressU);
			int? modifiedTexelV = GetTextureAddress(texV, height, samplerState.AddressV);

			if (modifiedTexelU == null || modifiedTexelV == null)
				return samplerState.BorderColor;

			return GetColor(level, (int) modifiedTexelU, (int) modifiedTexelV);
		}

		private ColorF GetLinear(SamplerState samplerState, int level, float texelX, float texelY)
		{
			int intTexelX = (int) texelX;
			int intTexelY = (int) texelY;

			float fracX = texelX - intTexelX;
			float fracY = texelY - intTexelY;

			ColorF c00 = GetColor(samplerState, level, intTexelX, intTexelY);
			ColorF c10 = GetColor(samplerState, level, intTexelX + 1, intTexelY);
			ColorF c01 = GetColor(samplerState, level, intTexelX, intTexelY + 1);
			ColorF c11 = GetColor(samplerState, level, intTexelX + 1, intTexelY + 1);

			ColorF cMinV = (c00 * (1 - fracX)) + (c10 * fracX);
			ColorF cMaxV = (c01 * (1 - fracX)) + (c11 * fracX);

			ColorF cFinal = (cMinV * (1 - fracY)) + (cMaxV * fracY);
			return cFinal;
		}

		private ColorF GetNearestNeighbor(SamplerState samplerState, int level, float texU, float texV)
		{
			return GetColor(samplerState, level, MathUtility.Floor(texU), MathUtility.Floor(texV));
		}

		private static int? GetTextureAddress(int value, int maxValue, TextureAddressMode textureAddressMode)
		{
			// If value is in the valid texture address range, return it straight away.
			if (value >= 0 && value < maxValue)
				return value;

			// Otherwise, we need to use the specified addressing mode.
			switch (textureAddressMode)
			{
				case TextureAddressMode.Border :
					return null;
				case TextureAddressMode.Clamp :
					return (value < 0) ? 0 : maxValue - 1;
				case TextureAddressMode.Mirror:
				{
					int temp = value % (2 * maxValue);
					if (temp < 0)
						temp += (2 * maxValue);
					return (temp > maxValue) ? (2 * maxValue) - temp : temp;
				}
				case TextureAddressMode.Wrap:
				{
					int temp = value % maxValue;
					if (temp < 0)
						temp += maxValue;
					return temp;
				}
				default :
					throw new NotSupportedException();
			}
		}
	}
}