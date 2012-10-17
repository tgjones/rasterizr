using SlimShader.ObjectModel.Tokens;

namespace SlimShader.Parser.Declarations
{
	public class SamplerDeclarationParser
	{
		private readonly DxbcReader _reader;

		public SamplerDeclarationParser(DxbcReader reader)
		{
			_reader = reader;
		}

		public DeclareSamplerToken Parse()
		{
			return new DeclareSamplerToken
			{
				SamplerMode = Decoder.DecodeSamplerMode(_reader.ReadAndMoveNext()),
				Operand = new OperandParser(_reader).Parse()
			};
		}
	}
}