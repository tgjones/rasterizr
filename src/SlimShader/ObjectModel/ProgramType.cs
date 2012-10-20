namespace SlimShader.ObjectModel
{
	public enum ProgramType : ushort
	{
		PixelShader = 0,
		VertexShader = 1,
		GeometryShader = 2,

		// Below are shaders new in DX 11

		HullShader = 3,
		DomainShader = 4,
		ComputeShader = 5
	}
}