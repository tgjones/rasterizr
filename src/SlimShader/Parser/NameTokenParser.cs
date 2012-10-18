using SlimShader.IO;
using SlimShader.ObjectModel;

namespace SlimShader.Parser
{
	/// <summary>
	/// Name Token (NameToken) (used in declaration statements)
	///
	/// [15:00] D3D10_SB_NAME enumeration
	/// [31:16] Reserved, 0
	/// </summary>
	public class NameTokenParser : BytecodeParser<SystemValueName>
	{
		public NameTokenParser(BytecodeReader reader)
			: base(reader)
		{
		}

		public override SystemValueName Parse()
		{
			uint token = Reader.ReadUInt32();
			return token.DecodeValue<SystemValueName>(0, 15);
		}
	}
}