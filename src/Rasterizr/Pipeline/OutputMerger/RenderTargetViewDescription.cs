using System;
using System.Runtime.InteropServices;
using Rasterizr.Resources;

namespace Rasterizr.Pipeline.OutputMerger
{
	[StructLayout(LayoutKind.Explicit)]
	public struct RenderTargetViewDescription
	{
		internal static RenderTargetViewDescription CreateDefault(Resource resource)
		{
			var result = new RenderTargetViewDescription();
			switch (resource.ResourceType)
			{
				case ResourceType.Buffer:
					result.Dimension = RenderTargetViewDimension.Buffer;
					break;
				case ResourceType.Texture1D:
					result.Dimension = RenderTargetViewDimension.Texture1D;
					break;
				case ResourceType.Texture2D:
					result.Dimension = RenderTargetViewDimension.Texture2D;
					break;
				case ResourceType.Texture3D:
					result.Dimension = RenderTargetViewDimension.Texture3D;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return result;
		}

		[FieldOffset(0)]
		public Format Format;

		[FieldOffset(4)]
		public RenderTargetViewDimension Dimension;

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

		[StructLayout(LayoutKind.Explicit)]
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

		public struct Texture1DResource
		{
			public int MipSlice;
		}

		public struct Texture1DArrayResource
		{
			public int MipSlice;
			public int FirstArraySlice;
			public int ArraySize;
		}

		public struct Texture2DResource
		{
			public int MipSlice;
		}

		public struct Texture2DArrayResource
		{
			public int MipSlice;
			public int FirstArraySlice;
			public int ArraySize;
		}

		public struct Texture2DMultisampledResource
		{
			public int UnusedFieldNothingToDefine;
		}

		public struct Texture2DMultisampledArrayResource
		{
			public int FirstArraySlice;
			public int ArraySize;
		}

		public struct Texture3DResource
		{
			public int MipSlice;
			public int FirstDepthSlice;
			public int DepthSliceCount;
		}
	}
}