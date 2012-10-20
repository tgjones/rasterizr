using System.ComponentModel;

namespace SlimShader.ObjectModel
{
	public enum ResourceDimension
	{
		Unknown = 0,
		Buffer = 1,

		[Description("texture1d")]
		Texture1D = 2,

		[Description("texture2d")]
		Texture2D = 3,

		[Description("texture2dms")]
		Texture2DMultiSampled = 4,

		[Description("texture3d")]
		Texture3D = 5,

		[Description("texturecube")]
		TextureCube = 6,

		[Description("texture1darray")]
		Texture1DArray = 7,

		[Description("texture2darray")]
		Texture2DArray = 8,

		[Description("texture2dmsarray")]
		Texture2DMultiSampledArray = 9,

		[Description("texturecubearray")]
		TextureCubeArray = 10,

		RawBuffer = 11,
		StructuredBuffer = 12,
	}
}