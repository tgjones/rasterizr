namespace SlimShader.ObjectModel
{
	public enum OperandIndexDimension
	{
		D3D10_SB_OPERAND_INDEX_0D = 0, // e.g. Position
		D3D10_SB_OPERAND_INDEX_1D = 1, // Most common.  e.g. Temp registers.
		D3D10_SB_OPERAND_INDEX_2D = 2, // e.g. Geometry Program Input registers.
		D3D10_SB_OPERAND_INDEX_3D = 3, // 3D rarely if ever used.
	}
}