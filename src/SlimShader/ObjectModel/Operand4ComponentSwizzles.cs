namespace SlimShader.ObjectModel
{
	/// <summary>
	/// ENCODER/DECODER MACROS: Various common swizzle patterns
	/// (noswizzle and replicate of each channels)
	/// </summary>
	public static class Operand4ComponentSwizzles
	{
		/// <summary>
		/// ENCODER MACRO: Generate a 4 component swizzle given
		/// 4 D3D10_SB_4_COMPONENT_NAME source values for dest
		/// components x, y, z, w respectively.
		/// </summary>
		/// <param name="xSrc"></param>
		/// <param name="ySrc"></param>
		/// <param name="zSrc"></param>
		/// <param name="wSrc"></param>
		/// <returns></returns>
		private static uint EncodeOperand4ComponentSwizzle(
			Operand4ComponentName xSrc, Operand4ComponentName ySrc,
			Operand4ComponentName zSrc, Operand4ComponentName wSrc)
		{
			const int mask = 3;
			const int shift = 4;
			return (((uint) xSrc & mask)
				| (((uint) ySrc & mask) << 2)
				| (((uint) zSrc & mask) << 4)
				| (((uint) wSrc & mask) << 6));
			//<< shift;
		}

		public static readonly uint NoSwizzle = EncodeOperand4ComponentSwizzle(
			Operand4ComponentName.D3D10_SB_4_COMPONENT_X, Operand4ComponentName.D3D10_SB_4_COMPONENT_Y,
			Operand4ComponentName.D3D10_SB_4_COMPONENT_Z, Operand4ComponentName.D3D10_SB_4_COMPONENT_W);

		public static readonly uint ReplicateX = EncodeOperand4ComponentSwizzle(
			Operand4ComponentName.D3D10_SB_4_COMPONENT_X, Operand4ComponentName.D3D10_SB_4_COMPONENT_X,
			Operand4ComponentName.D3D10_SB_4_COMPONENT_X, Operand4ComponentName.D3D10_SB_4_COMPONENT_X);

		public static readonly uint ReplicateY = EncodeOperand4ComponentSwizzle(
			Operand4ComponentName.D3D10_SB_4_COMPONENT_Y, Operand4ComponentName.D3D10_SB_4_COMPONENT_Y,
			Operand4ComponentName.D3D10_SB_4_COMPONENT_Y, Operand4ComponentName.D3D10_SB_4_COMPONENT_Y);

		public static readonly uint ReplicateZ = EncodeOperand4ComponentSwizzle(
			Operand4ComponentName.D3D10_SB_4_COMPONENT_Z, Operand4ComponentName.D3D10_SB_4_COMPONENT_Z,
			Operand4ComponentName.D3D10_SB_4_COMPONENT_Z, Operand4ComponentName.D3D10_SB_4_COMPONENT_Z);

		public static readonly uint ReplicateW = EncodeOperand4ComponentSwizzle(
			Operand4ComponentName.D3D10_SB_4_COMPONENT_W, Operand4ComponentName.D3D10_SB_4_COMPONENT_W,
			Operand4ComponentName.D3D10_SB_4_COMPONENT_W, Operand4ComponentName.D3D10_SB_4_COMPONENT_W);

		public static readonly uint ReplaceRed = ReplicateX;
		public static readonly uint ReplaceGreen = ReplicateY;
		public static readonly uint ReplaceBlue = ReplicateZ;
		public static readonly uint ReplaceAlpha = ReplicateW;
	}
}