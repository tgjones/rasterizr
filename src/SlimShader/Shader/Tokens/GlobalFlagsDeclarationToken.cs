using SlimShader.IO;
using SlimShader.Util;

namespace SlimShader.Shader.Tokens
{
	/// <summary>
	/// Global Flags Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_GLOBAL_FLAGS
	/// [11:11] Refactoring allowed if bit set.
	/// [12:12] Enable double precision float ops.
	/// [13:13] Force early depth-stencil test.
	/// [14:14] Enable RAW and structured buffers in non-CS 4.x shaders.
	/// [15:15] Skip optimizations of shader IL when translating to native code
	/// [16:16] Enable minimum-precision data types
	/// [17:17] Enable 11.1 double-precision floating-point instruction extensions
	/// [18:18] Enable 11.1 non-double instruction extensions
	/// [23:19] Reserved for future flags.
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	// OpcodeToken0 is followed by no operands.
	/// </summary>
	public class GlobalFlagsDeclarationToken : DeclarationToken
	{
		/// <summary>
		/// Refactoring allowed if bit set.
		/// </summary>
		public bool RefactoringAllowed { get; private set; }

		/// <summary>
		/// Enable double precision float ops.
		/// </summary>
		public bool EnableDoublePrecisionFloatOps { get; private set; }

		/// <summary>
		/// Force early depth-stencil test.
		/// </summary>
		public bool ForceEarlyDepthStencilTest { get; private set; }

		/// <summary>
		/// Enable RAW and structured buffers in non-CS 4.x shaders.
		/// </summary>
		public bool EnableRawAndStructuredBuffersInNonCsShaders { get; private set; }

		/// <summary>
		/// Skip optimizations of shader IL when translating to native code
		/// </summary>
		public bool SkipOptimizationsOfShaderIl { get; private set; }

		/// <summary>
		/// Enable minimum-precision data types
		/// </summary>
		public bool EnableMinimumPrecisionDataTypes { get; private set; }

		/// <summary>
		/// Enable 11.1 double-precision floating-point instruction extensions
		/// </summary>
		public bool Enable11Point1DoublePrecisionInstructionExtensions { get; private set; }

		/// <summary>
		/// Enable 11.1 non-double instruction extensions
		/// </summary>
		public bool Enable11Point1NonDoubleInstructionExtensions { get; private set; }

		public static GlobalFlagsDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			return new GlobalFlagsDeclarationToken
			{
				RefactoringAllowed = (token0.DecodeValue(11, 11) == 1),
				EnableDoublePrecisionFloatOps = (token0.DecodeValue(12, 12) == 1),
				ForceEarlyDepthStencilTest = (token0.DecodeValue(13, 13) == 1),
				EnableRawAndStructuredBuffersInNonCsShaders = (token0.DecodeValue(14, 14) == 1),
				SkipOptimizationsOfShaderIl = (token0.DecodeValue(15, 15) == 1),
				EnableMinimumPrecisionDataTypes = (token0.DecodeValue(16, 16) == 1),
				Enable11Point1DoublePrecisionInstructionExtensions = (token0.DecodeValue(17, 17) == 1),
				Enable11Point1NonDoubleInstructionExtensions = (token0.DecodeValue(18, 18) == 1)
			};
		}
	}
}