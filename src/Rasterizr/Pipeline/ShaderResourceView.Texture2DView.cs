using System;
using Rasterizr.Math;
using Rasterizr.Resources;
using SlimShader;
using SlimShader.VirtualMachine.Resources;

namespace Rasterizr.Pipeline
{
	public partial class ShaderResourceView
	{
		private class Texture2DView : InnerResourceView
		{
			private readonly Texture2D.Texture2DSubresource[] _subresources;
			private readonly Format _format;

			public Texture2DView(Texture2D resource, ShaderResourceViewDescription.Texture2DResource description, Format format)
			{
				_subresources = new Texture2D.Texture2DSubresource[description.MipLevels];
				for (int i = description.MostDetailedMip; i < description.MostDetailedMip + description.MipLevels; i++)
					_subresources[i] = resource.GetSubresource(0, i);
				_format = format;
			}

            public override float CalculateLevelOfDetail(ISamplerState sampler, ref Number4 ddx, ref Number4 ddy)
			{
				var subresource = _subresources[0];
				int width = subresource.Width;
				int height = subresource.Height;
				float xBound2 = width * width;
				float yBound2 = height * height;

				float dudx2 = ddx.Number0.Float * ddx.Number0.Float * xBound2;
				float dvdx2 = ddx.Number1.Float * ddx.Number1.Float * yBound2;
				float dudy2 = ddy.Number0.Float * ddy.Number0.Float * xBound2;
				float dvdy2 = ddy.Number1.Float * ddy.Number1.Float * yBound2;

				// Proportional to the amount of a texel on display in a single pixel
				float pixelSizeTexelRatio2 = System.Math.Max(dudx2 + dvdx2, dudy2 + dvdy2);

				// Uses formula for p410 of Essential Mathematics for Games and Interactive Applications
				return 0.5f * MathUtility.Log2(pixelSizeTexelRatio2);
			}

            public override Number4 SampleLevel(ISamplerState sampler, ref Number4 location, float lod)
            {
                var samplerState = (SamplerStateDescription) sampler.Description; // TODO: This isn't nice.
                switch (samplerState.Filter)
                {
                    case Filter.MinPointMagLinearMipPoint :
                    case Filter.MinLinearMagMipPoint:
                    case Filter.MinMagLinearMipPoint:
                    case Filter.MinMagMipPoint:
                        {
                            // Calculate nearest mipmap level.
                            var nearestLevel = MathUtility.Round(lod);
                            return GetFilteredColor(samplerState.Filter, true, nearestLevel, ref samplerState, 
                                location.Number0.Float, location.Number1.Float)
                                .ToNumber4();
                        }
                    case Filter.MinLinearMagPointMipLinear:
                    case Filter.MinMagMipLinear:
                    case Filter.MinMagPointMipLinear:
                    case Filter.MinPointMagMipLinear:
                        {
                            // Calculate nearest two levels and linearly filter between them.
                            var nearestLevelInt = (int) lod;
                            var d = lod - nearestLevelInt;
                            var c1 = GetFilteredColor(samplerState.Filter, true, nearestLevelInt, ref samplerState,
                                location.Number0.Float, location.Number1.Float);
                            var c2 = GetFilteredColor(samplerState.Filter, true, nearestLevelInt + 1, ref samplerState,
                                location.Number0.Float, location.Number1.Float);
                            return ((c1 * (1 - d)) + (c2 * d)).ToNumber4();
                        }
                    default:
                        throw new NotSupportedException();
                }
			}

			public override Color4F GetDataIndex(SamplerStateDescription sampler, float u, float v, float w)
			{
				return GetFilteredColor(sampler.Filter, true, 0, ref sampler, u, v);
			}

			private Color4F GetFilteredColor(Filter filter, bool minifying, int level, ref SamplerStateDescription samplerState, float u, float v)
			{
				level = MathUtility.Clamp(level, 0, _subresources.Length - 1);

				var subresource = _subresources[level];
				int width = subresource.Width;
				int height = subresource.Height;
				u *= width;
				v *= height;

				// Minifying
				if (minifying)
					switch (filter)
					{
						case Filter.MinMagMipPoint:
						case Filter.MinMagPointMipLinear:
                        case Filter.MinPointMagLinearMipPoint :
						case Filter.MinPointMagMipLinear:
							return GetNearestNeighbor(ref samplerState, level, u, v);
						case Filter.MinLinearMagMipPoint:
						case Filter.MinLinearMagPointMipLinear:
						case Filter.MinMagLinearMipPoint:
						case Filter.MinMagMipLinear:
							return GetLinear(ref samplerState, level, u, v);
						default:
							throw new NotSupportedException();
					}

				// Magnifying
				switch (filter)
				{
					case Filter.MinLinearMagMipPoint:
					case Filter.MinLinearMagPointMipLinear:
					case Filter.MinMagMipPoint:
					case Filter.MinMagPointMipLinear:
						return GetNearestNeighbor(ref samplerState, level, u, v);
					case Filter.MinMagLinearMipPoint:
					case Filter.MinMagMipLinear:
                    case Filter.MinPointMagLinearMipPoint:
					case Filter.MinPointMagMipLinear:
						return GetLinear(ref samplerState, level, u, v);
					default:
						throw new NotSupportedException();
				}
			}

			private Color4F GetLinear(ref SamplerStateDescription samplerState, int level, float texelX, float texelY)
			{
				int intTexelX = (int) texelX;
				int intTexelY = (int) texelY;

				float fracX = texelX - intTexelX;
				float fracY = texelY - intTexelY;

				Color4F c00 = GetColor(ref samplerState, level, intTexelX, intTexelY);
				Color4F c10 = GetColor(ref samplerState, level, intTexelX + 1, intTexelY);
				Color4F c01 = GetColor(ref samplerState, level, intTexelX, intTexelY + 1);
				Color4F c11 = GetColor(ref samplerState, level, intTexelX + 1, intTexelY + 1);

				Color4F cMinV = (c00 * (1 - fracX)) + (c10 * fracX);
				Color4F cMaxV = (c01 * (1 - fracX)) + (c11 * fracX);

				Color4F cFinal = (cMinV * (1 - fracY)) + (cMaxV * fracY);
				return cFinal;
			}

			private Color4F GetNearestNeighbor(ref SamplerStateDescription samplerState, int level, float texU, float texV)
			{
				return GetColor(ref samplerState, level, MathUtility.Floor(texU), MathUtility.Floor(texV));
			}

			private Color4F GetColor(ref SamplerStateDescription samplerState, int level, int texU, int texV)
			{
				var subresource = _subresources[level];

				int? modifiedTexelU = GetTextureAddress(texU, subresource.Width, samplerState.AddressU);
				int? modifiedTexelV = GetTextureAddress(texV, subresource.Height, samplerState.AddressV);

				if (modifiedTexelU == null || modifiedTexelV == null)
					return samplerState.BorderColor;

				var dataIndex = new DataIndex
				{
					Data = subresource.Data,
					Offset = subresource.CalculateByteOffset(modifiedTexelU.Value, modifiedTexelV.Value)
				};

				return FormatHelper.Convert(_format, dataIndex);
			}

			private static int? GetTextureAddress(int value, int maxValue, TextureAddressMode textureAddressMode)
			{
				// If value is in the valid texture address range, return it straight away.
				if (value >= 0 && value < maxValue)
					return value;

				// Otherwise, we need to use the specified addressing mode.
				switch (textureAddressMode)
				{
					case TextureAddressMode.Border:
						return null;
					case TextureAddressMode.Clamp:
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
					default:
						throw new ArgumentOutOfRangeException("textureAddressMode");
				}
			}
		}
	}
}