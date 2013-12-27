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
				case ResourceType.Texture1D:
			    {
                    var texture = (Texture1D) resource;
                    if (texture.Description.ArraySize > 1)
                    {
                        result.Dimension = ShaderResourceViewDimension.Texture1DArray;
                        result.Texture1DArray.ArraySize = texture.Description.ArraySize;
                        result.Texture1DArray.FirstArraySlice = 0;
                        result.Texture1DArray.MostDetailedMip = 0;
                        result.Texture1DArray.MipLevels = texture.Description.MipLevels;
                    }
                    else
                    {
                        result.Dimension = ShaderResourceViewDimension.Texture1D;
                        result.Texture1D.MostDetailedMip = 0;
                        result.Texture1D.MipLevels = texture.Description.MipLevels;
                    }
                    break;
			    }
				case ResourceType.Texture2D:
			    {
			        var texture = (Texture2D) resource;
			        if (texture.Description.ArraySize > 1)
			        {
			            result.Dimension = ShaderResourceViewDimension.Texture2DArray;
			            result.Texture2DArray.ArraySize = texture.Description.ArraySize;
			            result.Texture2DArray.FirstArraySlice = 0;
			            result.Texture2DArray.MostDetailedMip = 0;
			            result.Texture2DArray.MipLevels = texture.Description.MipLevels;
			        }
			        else
			        {
			            result.Dimension = ShaderResourceViewDimension.Texture2D;
			            result.Texture2D.MostDetailedMip = 0;
			            result.Texture2D.MipLevels = texture.Description.MipLevels;
			        }
			        break;
			    }
			    case ResourceType.Texture3D:
					result.Dimension = ShaderResourceViewDimension.Texture3D;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return result;
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

		[FieldOffset(0)]
		public ShaderResourceViewDimension Dimension;

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

		[FieldOffset(4)]
		public TextureCubeResource TextureCube;

		[FieldOffset(4)]
		public TextureCubeArrayResource TextureCubeArray;
	}
}