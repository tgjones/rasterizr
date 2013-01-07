using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Rasterizr.Math;
using Rasterizr.Resources;
using Rasterizr.Util;

namespace Rasterizr
{
	public static class FormatHelper
	{
		private static readonly int[] sizeOfInBits = new int[256];

		public static int SizeOfInBytes(Format format)
		{
			return SizeOfInBits(format) / 8;
		}

		public static int SizeOfInBits(Format format)
		{
			return sizeOfInBits[(int) format];
		}

		/// <summary>
		/// Clamps the specified color to the range specified by the format parameter.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public static Color4F Clamp(Color4F value, Format format)
		{
			switch (format)
			{
				case Format.B8G8R8A8_UInt:
					return FormatB8G8R8A8UInt.FromColor(value).ToColor();
				case Format.R8G8B8A8_UInt:
					return value.ToColor4().ToColor4F();
				default :
					throw new NotSupportedException();
			}
		}

		internal static void Convert(Format format, Color4F source, DataIndex destination)
		{
			switch (format)
			{
				case Format.B8G8R8A8_UInt:
				{
					var converted = FormatB8G8R8A8UInt.FromColor(source);
					Utilities.ToByteArray(ref converted, destination.Data, destination.Offset);
					break;
				}
			}
		}

		internal static Color4F Convert(Format format, DataIndex source)
		{
			switch (format)
			{
				case Format.B8G8R8A8_UInt:
				{
					FormatB8G8R8A8UInt converted;
					Utilities.FromByteArray(out converted, source.Data, source.Offset, SizeOfInBytes(format));
					return converted.ToColor();
				}
				case Format.R8G8B8A8_UInt:
				{
					Color4 converted;
					Utilities.FromByteArray(out converted, source.Data, source.Offset, SizeOfInBytes(format));
					return converted.ToColor4F();
				}
				default:
				{
					throw new NotSupportedException();
				}
			}
		}

		internal static void Clear(TextureBase.ISubresource subresource, Format format, ref Color4F color)
		{
			switch (format)
			{
				case Format.B8G8R8A8_UInt:
				{
					var converted = FormatB8G8R8A8UInt.FromColor(color);
					subresource.Clear(ref converted);
					break;
				}
				case Format.R8G8B8A8_UInt:
				{
					var converted = color.ToColor4();
					subresource.Clear(ref converted);
					break;
				}
				default:
				{
					throw new NotSupportedException();
				}
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct FormatB8G8R8A8UInt
		{
			public static FormatB8G8R8A8UInt FromColor(Color4F value)
			{
				var converted = value.ToColor4();
				return new FormatB8G8R8A8UInt
				{
					B = converted.B,
					G = converted.G,
					R = converted.R,
					A = converted.A
				};
			}

			public byte B;
			public byte G;
			public byte R;
			public byte A;

			public Color4F ToColor()
			{
				return new Color4F(
					R * 255 / 1.0f,
					G * 255 / 1.0f,
					B * 255 / 1.0f,
					A * 255 / 1.0f);
			}
		}

		static FormatHelper()
		{
			InitFormat(new[] { Format.A8_UNorm, Format.R8_SInt, Format.R8_SNorm, Format.R8_Typeless, Format.R8_UInt, Format.R8_UNorm }, 8);

			InitFormat(new[] { 
                Format.B5G5R5A1_UNorm,
                Format.B5G6R5_UNorm,
                Format.D16_UNorm,
                Format.R16_Float,
                Format.R16_SInt,
                Format.R16_SNorm,
                Format.R16_Typeless,
                Format.R16_UInt,
                Format.R16_UNorm,
                Format.R8G8_SInt,
                Format.R8G8_SNorm,
                Format.R8G8_Typeless,
                Format.R8G8_UInt,
                Format.R8G8_UNorm,
                Format.B4G4R4A4_UNorm
            }, 16);

			InitFormat(new[] { 
                Format.B8G8R8X8_Typeless,
                Format.B8G8R8X8_UNorm,
                Format.B8G8R8X8_UNorm_SRgb,
                Format.D24_UNorm_S8_UInt,
                Format.D32_Float,
                Format.D32_Float_S8X24_UInt,
                Format.G8R8_G8B8_UNorm,
                Format.R10G10B10_Xr_Bias_A2_UNorm,
                Format.R10G10B10A2_Typeless,
                Format.R10G10B10A2_UInt,
                Format.R10G10B10A2_UNorm,
                Format.R11G11B10_Float,
                Format.R16G16_Float,
                Format.R16G16_SInt,
                Format.R16G16_SNorm,
                Format.R16G16_Typeless,
                Format.R16G16_UInt,
                Format.R16G16_UNorm,
                Format.R24_UNorm_X8_Typeless,
                Format.R24G8_Typeless,
                Format.R32_Float,
                Format.R32_Float_X8X24_Typeless,
                Format.R32_SInt,
                Format.R32_Typeless,
                Format.R32_UInt,
                Format.R8G8_B8G8_UNorm,
                Format.R8G8B8A8_SInt,
                Format.R8G8B8A8_SNorm,
                Format.R8G8B8A8_Typeless,
                Format.R8G8B8A8_UInt,
                Format.R8G8B8A8_UNorm,
                Format.R8G8B8A8_UNorm_SRgb,
                Format.B8G8R8A8_Typeless,
				Format.B8G8R8A8_UInt,
                Format.B8G8R8A8_UNorm,
                Format.B8G8R8A8_UNorm_SRgb,
                Format.R9G9B9E5_Sharedexp,
                Format.X24_Typeless_G8_UInt,
                Format.X32_Typeless_G8X24_UInt,
            }, 32);

			InitFormat(new[] { 
                Format.R16G16B16A16_Float,
                Format.R16G16B16A16_SInt,
                Format.R16G16B16A16_SNorm,
                Format.R16G16B16A16_Typeless,
                Format.R16G16B16A16_UInt,
                Format.R16G16B16A16_UNorm,
                Format.R32G32_Float,
                Format.R32G32_SInt,
                Format.R32G32_Typeless,
                Format.R32G32_UInt,
                Format.R32G8X24_Typeless,
            }, 64);

			InitFormat(new[] { 
                Format.R32G32B32_Float,
                Format.R32G32B32_SInt,
                Format.R32G32B32_Typeless,
                Format.R32G32B32_UInt,
            }, 96);

			InitFormat(new[] { 
                Format.R32G32B32A32_Float,
                Format.R32G32B32A32_SInt,
                Format.R32G32B32A32_Typeless,
                Format.R32G32B32A32_UInt,
            }, 128);
		}

		private static void InitFormat(IEnumerable<Format> formats, int bitCount)
		{
			foreach (var format in formats)
				sizeOfInBits[(int) format] = bitCount;
		}
	}
}