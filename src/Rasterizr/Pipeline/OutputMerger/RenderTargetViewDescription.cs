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
			    {
                    var texture = (Texture1D) resource;
                    if (texture.Description.ArraySize > 1)
                    {
                        result.Dimension = RenderTargetViewDimension.Texture1DArray;
                        result.Texture1DArray.ArraySize = texture.Description.ArraySize;
                        result.Texture1DArray.FirstArraySlice = 0;
                        result.Texture1DArray.MipSlice = 0;
                    }
                    else
                    {
                        result.Dimension = RenderTargetViewDimension.Texture1D;
                        result.Texture1D.MipSlice = 0;
                    }
                    break;
			    }
				case ResourceType.Texture2D:
			    {
                    var texture = (Texture2D) resource;
                    if (texture.Description.ArraySize > 1)
                    {
                        result.Dimension = RenderTargetViewDimension.Texture2DArray;
                        result.Texture2DArray.ArraySize = texture.Description.ArraySize;
                        result.Texture2DArray.FirstArraySlice = 0;
                        result.Texture2DArray.MipSlice = 0;
                    }
                    else
                    {
                        result.Dimension = RenderTargetViewDimension.Texture2D;
                        result.Texture2D.MipSlice = 0;
                    }
			        break;
			    }
			    case ResourceType.Texture3D:
			    {
                    var texture = (Texture3D) resource;
			        result.Dimension = RenderTargetViewDimension.Texture3D;
			        result.Texture3D.DepthSliceCount = texture.Description.Depth;
			        result.Texture3D.FirstDepthSlice = 0;
			        result.Texture3D.MipSlice = 0;
			        break;
			    }
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