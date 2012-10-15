using System.IO;
using SlimShader.ObjectModel;

namespace SlimShader
{
	public class DxbcReader : BinaryReader
	{
		public DxbcReader(Stream input)
			: base(input)
		{
		}

		internal static uint GetFourCc(char a, char b, char c, char d)
		{
			return a | ((uint) (b << 8)) | ((uint) c << 16) | ((uint) d << 24);
		}

		public DxbcContainerHeader ReadContainerHeader()
		{
			uint fourCcDxbc = GetFourCc('D', 'X', 'B', 'C');
			uint fourCc = ReadUInt32();
			if (fourCc != fourCcDxbc)
				throw new ParseException("Invalid FourCC");

			var uniqueKey = new uint[4];
			uniqueKey[0] = ReadUInt32();
			uniqueKey[1] = ReadUInt32();
			uniqueKey[2] = ReadUInt32();
			uniqueKey[3] = ReadUInt32();

			return new DxbcContainerHeader
			{
				FourCc = fourCc,
				UniqueKey = uniqueKey,
				One = ReadUInt32(),
				TotalSize = ReadUInt32(),
				ChunkCount = ReadUInt32()
			};
		}

		public DxbcChunkHeader ReadChunkHeader()
		{
			return new DxbcChunkHeader
			{
				FourCc = ReadUInt32(),
				Size = ReadUInt32()
			};
		}

		private static byte DecodeProgramMajorVersion(uint token)
		{
			return (byte) ((token & 0x000000f0) >> 4);
		}

		private static byte DecodeProgramMinorVersion(uint token)
		{
			return (byte) ((token & 0x0000000f));
		}

		private static ShaderType DecodeShaderType(uint token)
		{
			return (ShaderType) ((token & 0xffff0000) >> 16);
		}

		public ShaderVersion ReadShaderVersion()
		{
			uint token = ReadUInt32();
			return new ShaderVersion
			{
				MajorVersion = DecodeProgramMajorVersion(token),
				MinorVersion = DecodeProgramMinorVersion(token),
				ShaderType = DecodeShaderType(token)
			};
		}

		private static Opcode DecodeOperandType(uint token)
		{
			return (Opcode) ((token & 0x000ff000) >> 12);
		}

		private static uint DecodeInstructionLength(uint token)
		{
			return (token & 0x7f000000) >> 24;
		}

		private static bool DecodeIsOpcodeExtended(uint token)
		{
			return ((token & 0x80000000) >> 31) == 1;
		}

		public OpcodeHeader ReadOpcodeHeader()
		{
			uint token = ReadUInt32();
			return new OpcodeHeader
			{
				Opcode = DecodeOperandType(token),
				Length = DecodeInstructionLength(token),
				IsExtended = DecodeIsOpcodeExtended(token)
			};
		}
	}
}