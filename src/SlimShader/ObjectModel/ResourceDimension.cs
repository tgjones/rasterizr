namespace SlimShader.ObjectModel
{
	public enum ResourceDimension
	{
		Unknown = 0,
		Buffer = 1,
		Texture1D = 2,
		Texture2D = 3,
		Texture2DMultiSampled = 4,
		Texture3D = 5,
		TextureCube = 6,
		Texture1DArray = 7,
		Texture2DArray = 8,
		Texture2DMultiSampledArray = 9,
		TextureCubeArray = 10,
		RawBuffer = 11,
		StructuredBuffer = 12,
	}
}