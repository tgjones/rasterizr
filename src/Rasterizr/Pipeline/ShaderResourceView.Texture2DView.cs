using System;
using Rasterizr.Math;
using Rasterizr.Resources;

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

			public override Color4F GetDataIndex(SamplerStateDescription sampler, float u, float v, float w)
			{
				var subresource = _subresources[0];

				int texU = (int) (u * subresource.Width);
				int texV = (int) (v * subresource.Height);

				int? modifiedTexelU = GetTextureAddress(texU, subresource.Width, sampler.AddressU);
				int? modifiedTexelV = GetTextureAddress(texV, subresource.Height, sampler.AddressV);

				if (modifiedTexelU == null || modifiedTexelV == null)
					return sampler.BorderColor;

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