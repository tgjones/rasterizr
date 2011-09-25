using System;
using Nexus;
using Nexus.Graphics;

namespace Rasterizr.Core.ShaderCore
{
	public class Texture2D
	{
		private readonly ColorSurface _surface;
		private TextureMipMapLevel[] _levels;
		private readonly int _xBound;
		private readonly int _yBound;

		public Texture2D(string uri)
		{
			_surface = ColorSurfaceLoader.LoadFromFile(uri);

			//if (textureSource.Dimensions != 2)
			//	throw new ArgumentOutOfRangeException("uriResource", "Texture source for Texture2D must have 2 dimensions");

			//_xBound = textureSource.GetBound(0);
			//_yBound = textureSource.GetBound(1);
			_xBound = _surface.Width;
			_yBound = _surface.Height;

			CreateMipMaps();
		}

		private void CreateMipMaps()
		{
			int numLevels = (int) MathUtility.Log2(Math.Max(_xBound, _yBound)) + 1;
			_levels = new TextureMipMapLevel[numLevels];

			int width = _xBound / 2;
			int height = _yBound / 2;
			for (int level = 1; level < numLevels; ++level)
			{
				_levels[level] = new TextureMipMapLevel(level, width, height);
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

				width /= 2;
				height /= 2;
			}
		}

		private ColorF GetColor(int level, int x, int y)
		{
			if (level == 0)
				return _surface[x, y];
			return _levels[level].Texels[x, y];
		}

		public ColorF Sample(SamplerState samplerState, Point2D location)
		{
			// TODO: Need to calculate partial differentials.
			return SampleGrad(samplerState, location, Vector2D.Zero, Vector2D.Zero);
		}

		public ColorF SampleGrad(SamplerState samplerState, Point2D location, Vector2D ddx, Vector2D ddy)
		{
			float texU = location.X * _xBound;
			float texV = location.Y * _yBound;

			float xBound2 = _xBound * _xBound;
			float yBound2 = _yBound * _yBound;

			float dudx2 = ddx.X * ddx.X * xBound2;
			float dvdx2 = ddx.Y * ddx.Y * yBound2;
			float dudy2 = ddy.X * ddy.X * xBound2;
			float dvdy2 = ddy.Y * ddy.Y * yBound2;
			float pixelSizeTexelRatio2 = Math.Max(dudx2 + dvdx2, dudy2 + dvdy2); // Proportional to the amount of a texel on display in a single pixel
			if (pixelSizeTexelRatio2 < 1) // TODO: Is this correct?
				return GetFilteredColor(samplerState.MagFilter, 0, samplerState, location.X, location.Y);
			return GetMipMappedColor(samplerState, location.X, location.Y, pixelSizeTexelRatio2);
		}

		private ColorF GetMipMappedColor(SamplerState samplerState, float texU, float texV, float maxDifferentials2)
		{
			switch (samplerState.MipFilter)
			{
				case TextureMipMapFilter.Nearest:
				{
					// Calculate nearest mipmap level.
					int nearestLevel = MathUtility.Round(CalculateLevel(maxDifferentials2));
					return GetFilteredColor(samplerState.MinFilter, nearestLevel, samplerState, texU, texV);
				}
				case TextureMipMapFilter.Linear:
				{
					// Calculate nearest two levels and linearly filter between them.
					float nearestLevel = CalculateLevel(maxDifferentials2);
					int nearestLevelInt = (int) nearestLevel;
					float d = nearestLevel - nearestLevelInt;
					ColorF c1 = GetFilteredColor(samplerState.MinFilter, nearestLevelInt, samplerState, texU, texV);
					ColorF c2 = GetFilteredColor(samplerState.MinFilter, nearestLevelInt + 1, samplerState, texU, texV);
					return (c1 * (1 - d)) + (c2 * d);
				}
				case TextureMipMapFilter.None:
					return GetFilteredColor(samplerState.MinFilter, 0, samplerState, texU, texV);
				default :
					throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Uses formula for p410 of Essential Mathematics for Games and Interactive Applications
		/// </summary>
		/// <returns></returns>
		private static float CalculateLevel(float maxDifferentials2)
		{
			return 0.5f * MathUtility.Log2(maxDifferentials2);
		}

		private int GetWidth(int level)
		{
			if (level == 0)
				return _xBound;
			return _levels[level].Width;
		}

		private int GetHeight(int level)
		{
			if (level == 0)
				return _yBound;
			return _levels[level].Height;
		}

		private ColorF GetFilteredColor(TextureFilter filter, int level, SamplerState samplerState, float x, float y)
		{
			level = MathUtility.Clamp(level, 0, _levels.Length - 1);

			x *= GetWidth(level);
			y *= GetHeight(level);

			switch (filter)
			{
				case TextureFilter.Nearest:
					return GetNearestNeighbor(samplerState, level, x, y);
				case TextureFilter.Bilinear:
					return GetLinear(samplerState, level, x, y);
				default:
					throw new NotSupportedException();
			}
		}

		private ColorF GetColor(SamplerState samplerState, int level, int texU, int texV)
		{
			int? modifiedTexelU = GetTextureAddress(texU, GetWidth(level), samplerState.AddressU);
			int? modifiedTexelV = GetTextureAddress(texV, GetHeight(level), samplerState.AddressV);

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