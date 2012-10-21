using SlimShader.IO;
using SlimShader.Shader;

namespace SlimShader.InputOutputSignature
{
	public class SignatureParameterDescription
	{
		public string SemanticName { get; private set; }
		public uint SemanticIndex { get; private set; }
		public uint Register { get; private set; }
		public SystemValueName SystemValueType { get; private set; }
		public RegisterComponentType ComponentType { get; private set; }
		public ComponentMask Mask { get; private set; }
		public byte ReadWriteMask { get; private set; }
		public uint Stream { get; private set; }
		public MinPrecision MinPrecision { get; private set; }

		public static SignatureParameterDescription Parse(BytecodeReader reader, BytecodeReader parameterReader)
		{
			uint nameOffset = parameterReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int) nameOffset);

			return new SignatureParameterDescription
			{
				SemanticName = nameReader.ReadString(),
				SemanticIndex = parameterReader.ReadUInt32(),
				SystemValueType = (SystemValueName) parameterReader.ReadUInt32(),
				ComponentType = (RegisterComponentType) parameterReader.ReadUInt32(),
				Register = parameterReader.ReadUInt32(),
				Mask = (ComponentMask) parameterReader.ReadByte(),
				ReadWriteMask = parameterReader.ReadByte(),
				Stream = parameterReader.ReadByte(),
				MinPrecision = (MinPrecision) parameterReader.ReadByte()
			};
		}
	}
}