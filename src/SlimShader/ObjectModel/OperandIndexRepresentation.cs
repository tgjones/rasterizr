namespace SlimShader.ObjectModel
{
	public enum OperandIndexRepresentation
	{
		OPERAND_INDEX_IMMEDIATE32 = 0, // Extra DWORD
		OPERAND_INDEX_IMMEDIATE64 = 1, // 2 Extra DWORDs
		//   (HI32:LO32)
		OPERAND_INDEX_RELATIVE = 2, // Extra operand
		OPERAND_INDEX_IMMEDIATE32_PLUS_RELATIVE = 3, // Extra DWORD followed by
		//   extra operand
		OPERAND_INDEX_IMMEDIATE64_PLUS_RELATIVE = 4, // 2 Extra DWORDS
		//   (HI32:LO32) followed
		//   by extra operand
	}
}