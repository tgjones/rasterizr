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
		public RenderTargetViewDimension Dimension;

		[FieldOffset(4)]
		public Texture1DResource Texture1D;

		[FieldOffset(4)]
		public Texture1DArrayResource Texture1DArray;

		[FieldOffset(4)]
		public Texture2DResource Texture2D;

		[FieldOffset(4)]
		public Texture2DArrayResource Texture2DArray;

		[FieldOffset(4)]
		public Texture2DMultisampledResource Texture2DMS;

		[FieldOffset(4)]
		public Texture2DMultisampledArrayResource Texture2DMSArray;

		[FieldOffset(4)]
		public Texture3DResource Texture3D;

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