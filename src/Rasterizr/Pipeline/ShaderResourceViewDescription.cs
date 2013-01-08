using System;
using System.Runtime.InteropServices;
using Rasterizr.Resources;

namespace Rasterizr.Pipeline
{
	[StructLayout(LayoutKind.Explicit, Pack = 0)]
	public struct ShaderResourceViewDescription
	{
		internal static ShaderResourceViewDescription CreateDefault(Resource resource)
		{
			var result = new ShaderResourceViewDescription();
			switch (resource.ResourceType)
			{
				case ResourceType.Buffer:
					result.Dimension = ShaderResourceViewDimension.Buffer;
					break;
				case ResourceType.Texture1D:
					result.Dimension = ShaderResourceViewDimension.Texture1D;
					break;
				case ResourceType.Texture2D:
					result.Dimension = ShaderResourceViewDimension.Texture2D;
					result.Texture2D.MostDetailedMip = 0;
					result.Texture2D.MipLevels = ((Texture2D) resource).Description.MipLevels;
					break;
				case ResourceType.Texture3D:
					result.Dimension = ShaderResourceViewDimension.Texture3D;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return result;
		}

		[StructLayout(LayoutKind.Explicit, Pack = 0)]
		public struct BufferResource
		{
			[FieldOffset(0)]
			public int FirstElement;

			[FieldOffset(0)]
			public int ElementOffset;

			[FieldOffset(4)]
			public int ElementCount;

			[FieldOffset(4)]
			public int ElementWidth;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 0)]
		public struct Texture1DResource
		{
			public int MostDetailedMip;
			public int MipLevels;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 0)]
		public struct Texture1DArrayResource
		{
			public int MostDetailedMip;
			public int MipLevels;
			public int FirstArraySlice;
			public int ArraySize;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 0)]
		public struct Texture2DResource
		{
			public int MostDetailedMip;
			public int MipLevels;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 0)]
		public struct Texture2DArrayResource
		{
			public int MostDetailedMip;
			public int MipLevels;
			public int FirstArraySlice;
			public int ArraySize;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 0)]
		public struct Texture2DMultisampledResource
		{
			public int UnusedFieldNothingToDefine;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 0)]
		public struct Texture2DMultisampledArrayResource
		{
			public int FirstArraySlice;
			public int ArraySize;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 0)]
		public struct TextureCubeResource
		{
			public int MostDetailedMip;
			public int MipLevels;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 0)]
		public struct TextureCubeArrayResource
		{
			public int MostDetailedMip;
			public int MipLevels;
			public int First2DArrayFace;
			public int CubeCount;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 0)]
		public struct Texture3DResource
		{
			public int MostDetailedMip;
			public int MipLevels;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 0)]
		public struct ExtendedBufferResource
		{
			public int FirstElement;
			public int ElementCount;
			public ShaderResourceViewExtendedBufferFlags Flags;
		}

		[FieldOffset(0)]
		public Format Format;

		[FieldOffset(4)]
		public ShaderResourceViewDimension Dimension;

		[FieldOffset(8)]
		public BufferResource Buffer;

		[FieldOffset(8)]
		public Texture1DResource Texture1D;

		[FieldOffset(8)]
		public Texture1DArrayResource Texture1DArray;

		[FieldOffset(8)]
		public Texture2DResource Texture2D;

		[FieldOffset(8)]
		public Texture2DArrayResource Texture2DArray;

		[FieldOffset(8)]
		public Texture2DMultisampledResource Texture2DMS;

		[FieldOffset(8)]
		public Texture2DMultisampledArrayResource Texture2DMSArray;

		[FieldOffset(8)]
		public Texture3DResource Texture3D;

		[FieldOffset(8)]
		public TextureCubeResource TextureCube;

		[FieldOffset(8)]
		public TextureCubeArrayResource TextureCubeArray;

		[FieldOffset(8)]
		public ExtendedBufferResource BufferEx;
	}
}