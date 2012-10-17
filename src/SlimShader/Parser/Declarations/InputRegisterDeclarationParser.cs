using SlimShader.ObjectModel.Tokens;

namespace SlimShader.Parser.Declarations
{
	public class InputRegisterDeclarationParser
	{
		private readonly DxbcReader _reader;

		public InputRegisterDeclarationParser(DxbcReader reader)
		{
			_reader = reader;
		}

		public InputRegisterDeclaration Parse()
		{
			return new InputRegisterDeclaration
			{
				InterpolationMode = Decoder.DecodeInterpolationMode(_reader.ReadAndMoveNext()),
				Operand = new OperandParser(_reader).Parse()
			};
		}
	}
}