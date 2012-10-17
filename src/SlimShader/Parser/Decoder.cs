using SlimShader.ObjectModel;

namespace SlimShader.Parser
{
	/// <summary>
	/// Helps with decoding bit-shifted values from inside uint's.
	/// </summary>
	internal static class Decoder
	{
		/// <summary>
		/// DECODER MACRO: Retrieve program type from version token
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static ProgramType DecodeProgramType(uint token)
		{
			const uint mask = 0xffff0000;
			const int shift = 16;
			return (ProgramType)((token & mask) >> shift);
		}

		/// <summary>
		/// DECODER MACRO: Retrieve major version # from version token
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static byte DecodeProgramMajorVersion(uint token)
		{
			const int mask = 0x000000f0;
			const int shift = 4;
			return (byte) ((token & mask) >> shift);
		}

		/// <summary>
		/// DECODER MACRO: Retrieve minor version # from version token
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static byte DecodeProgramMinorVersion(uint token)
		{
			const int mask = 0x0000000f;
			return (byte) ((token & mask));
		}

		/// <summary>
		/// DECODER MACRO: Retrieve program length
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static uint DecodeProgramLength(uint token)
		{
			return token;
		}

		/// <summary>
		/// DECODER MACRO: Retrieve program opcode
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static OpcodeType DecodeOpcodeType(uint token)
		{
			const int mask = 0x00007ff;
			return (OpcodeType) (token & mask);
		}

		/// <summary>
		/// DECODER MACRO: Retrieve instruction length
		/// in # of DWORDs including the opcode token(s).
		/// The range is 1-127.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static byte DecodeInstructionLength(uint token)
		{
			const int mask = 0x7f000000;
			const int shift = 24;
			return (byte) ((token & mask) >> shift);
		}

		/// <summary>
		/// DECODER MACRO: Determine if the opcode is extended
		/// by an additional opcode token.  Currently there are no
		/// extended opcodes.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static bool DecodeIsOpcodeExtended(uint token)
		{
			const uint mask = 0x80000000;
			const int shift = 31;
			return ((token & mask) >> shift) == 1;
		}

		/// <summary>
		/// DECODER MACRO: Extract from OperandToken0 how many components
		//// the data vector referred to by the operand contains.
		//// (D3D10_SB_OPERAND_NUM_COMPONENTS enum)
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static OperandNumComponents DecodeOperandNumComponents(uint token)
		{
			const int mask = 0x00000003;
			return (OperandNumComponents) (token & mask);
		}

		/// <summary>
		/// DECODER MACRO: For an operand representing 4component data,
		/// extract from OperandToken0 the method for selecting data from
		/// the 4 components (D3D10_SB_OPERAND_4_COMPONENT_SELECTION_MODE).
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static Operand4ComponentSelectionMode DecodeOperand4ComponentSelectionMode(uint token)
		{
			const int mask = 0x0000000c;
			const int shift = 2;
			return (Operand4ComponentSelectionMode)((token & mask) >> shift);
		}

		/// <summary>
		/// DECODER MACRO: When 4 component selection mode is
		/// D3D10_SB_OPERAND_4_COMPONENT_MASK_MODE, this macro
		/// extracts from OperandToken0 the 4 component (xyzw) mask,
		/// as a field of D3D10_SB_OPERAND_4_COMPONENT_MASK_[X|Y|Z|W] flags.
		/// Alternatively, the D3D10_SB_OPERAND_4_COMPONENT_MASK_[X|Y|Z|W] masks
		/// can be tested on OperandToken0 directly, without this macro.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static ComponentMask DecodeOperand4ComponentMask(uint token)
		{
			const int mask = 0x000000f0;
			const int shift = 4;
			return (ComponentMask) ((token & mask) >> shift);
		}

		/// <summary>
		/// DECODER MACRO: When 4 component selection mode is
		/// D3D10_SB_OPERAND_4_COMPONENT_SWIZZLE_MODE, this macro
		/// extracts from OperandToken0 the 4 component swizzle,
		/// as a field of D3D10_SB_OPERAND_4_COMPONENT_MASK_[X|Y|Z|W] flags.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static uint DecodeOperand4ComponentSwizzle(uint token)
		{
			const int mask = 0x00000ff0;
			const int shift = 4;
			return (token & mask) >> shift;
		}

		/// <summary>
		/// DECODER MACRO: Pass a D3D10_SB_4_COMPONENT_NAME as "DestComp" in following
		/// macro to extract, from OperandToken0 or from a decoded swizzle,
		/// the swizzle source component (D3D10_SB_4_COMPONENT_NAME enum):
		/// </summary>
		/// <param name="token"></param>
		/// <param name="component"></param>
		/// <returns></returns>
		public static Operand4ComponentName DecodeOperand4ComponentSwizzleSource(uint token, byte component)
		{
			const int shift = 4;
			const int mask = 3;
			return (Operand4ComponentName) (((token) >> (shift + 2 * (component & mask))) & mask);
		}

		/// <summary>
		/// DECODER MACRO: When 4 component selection mode is
		/// D3D10_SB_OPERAND_4_COMPONENT_SELECT_1_MODE, this macro
		/// extracts from OperandToken0 a D3D10_SB_4_COMPONENT_NAME
		/// which picks one of the 4 components.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static Operand4ComponentName DecodeOperand4ComponentSelect1(uint token)
		{
			const int mask = 0x00000030;
			const int shift = 4;
			return (Operand4ComponentName) ((token & mask) >> shift);
		}

		/// <summary>
		/// DECODER MACRO: Determine operand type from OperandToken0.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static OperandType DecodeOperandType(uint token)
		{
			const int mask = 0x000ff000;
			const int shift = 12;
			return (OperandType) ((token & mask) >> shift);
		}

		/// <summary>
		/// DECODER MACRO: Determine operand index dimension from OperandToken0.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static OperandIndexDimension DecodeOperandIndexDimension(uint token)
		{
			const int mask = 0x00300000;
			const int shift = 20;
			return (OperandIndexDimension)((token & mask) >> shift);
		}

		/// <summary>
		/// DECODER MACRO: Determine from OperandToken0 what representation
		/// an operand index is provided as (D3D10_SB_OPERAND_INDEX_REPRESENTATION enum),
		/// for index dimension [0], [1] or [2], depending on D3D10_SB_OPERAND_INDEX_DIMENSION.
		/// </summary>
		/// <param name="dimension"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public static OperandIndexRepresentation DecodeOperandIndexRepresentation(
			byte dimension, uint token)
		{
			int shift = 22 + 3 * (dimension & 3);
			int mask = 0x3 << shift;
			return ((OperandIndexRepresentation) (((token) & mask) >> shift));
		}

		/// <summary>
		/// DECODER MACRO: Determine if the operand is extended
		/// by an additional opcode token.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static bool DecodeIsOperandExtended(uint token)
		{
			return DecodeIsOpcodeExtended(token);
		}

		/// <summary>
		/// DECODER MACRO: Given an extended operand
		/// token (OperandToken1), figure out what type
		/// of token it is (from D3D10_SB_EXTENDED_OPERAND_TYPE enum)
		/// to be able to interpret the rest of the token's contents.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static ExtendedOperandType DecodeExtendedOperandType(uint token)
		{
			const int mask = 0x0000003f;
			return (ExtendedOperandType)(token & mask);
		}

		/// <summary>
		/// DECODER MACRO: Given a D3D10_SB_EXTENDED_OPERAND_MODIFIER
		/// extended token (OperandToken1), determine the source modifier
		/// (D3D10_SB_OPERAND_MODIFIER enum)
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static OperandModifier DecodeExtendedOperandModifier(uint token)
		{
			const int mask = 0x00003fc0;
			const int shift = 6;
			return (OperandModifier)((token & mask) >> shift);
		}

		/// <summary>
		/// DECODER MACRO: Given a resource declaration token,
		/// (OpcodeToken0), determine the resource dimension
		/// (D3D10_SB_RESOURCE_DIMENSION enum)
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static ResourceDimension DecodeResourceDimension(uint token)
		{
			const int mask = 0x0000f800;
			const int shift = 11;
			return (ResourceDimension)((token & mask) >> shift);
		}

		/// <summary>
		/// DECODER MACRO: Given a resource declaration token,
		/// (OpcodeToken0), determine the resource sample count (1..127)
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static byte DecodeResourceSampleCount(uint token)
		{
			const int mask = 0x07F0000;
			const int shift = 16;
			return (byte) (((token) & mask) >> shift);
		}

		/// <summary>
		/// DECODER MACRO: Get the resource return type for component (0-3) from
		/// ResourceReturnTypeToken
		/// </summary>
		/// <param name="token"></param>
		/// <param name="component"> </param>
		/// <returns></returns>
		public static ResourceReturnType DecodeResourceReturnType(uint token, byte component)
		{
			const int mask = 0x0000000f;
			const int numBits = 0x00000004;
			return ((ResourceReturnType) (((token) >> component * numBits) & mask));
		}

		/// <summary>
		/// DECODER MACRO: Find out if a Constant Buffer is going to be indexed or not
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static SamplerMode DecodeSamplerMode(uint token)
		{
			const int mask = 0x00007800;
			const int shift = 11;

			return (SamplerMode) ((token & mask) >> shift);
		}

		/// <summary>
		/// DECODER MACRO: Find out interpolation mode for the input register
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static InterpolationMode DecodeInterpolationMode(uint token)
		{
			const int mask = 0x00007800;
			const int shift = 11;
			return (InterpolationMode) ((token & mask) >> shift);
		}
	}
}