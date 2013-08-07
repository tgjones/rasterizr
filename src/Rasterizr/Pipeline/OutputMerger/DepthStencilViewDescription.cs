using System;
using System.Runtime.InteropServices;
using Rasterizr.Resources;

namespace Rasterizr.Pipeline.OutputMerger
{
	[StructLayout(LayoutKind.Explicit, Pack = 0)]
	public struct DepthStencilViewDescription
	{
		internal static DepthStencilViewDescription CreateDefault(Resource resource)
		{
			var result = new DepthStencilViewDescription();
			switch (resource.ResourceType)
			{
				case ResourceType.Texture1D:
					result.Dimension = DepthStencilViewDimension.Texture1D;
					break;
				case ResourceType.Texture2D:
					result.Dimension = DepthStencilViewDimension.Texture2D;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return result;
		}

		public struct Texture2DResource
		{
			public int MipSlice;
		}

		public struct Texture1DArrayResource
		{
			public int MipSlice;
			public int FirstArraySlice;
			public int ArraySize;
		}

		public struct Texture1DResource
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

		[FieldOffset(0)]
		public DepthStencilViewDimension Dimension;

		[FieldOffset(4)]
		public DepthStencilViewFlags Flags;

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
	}
}