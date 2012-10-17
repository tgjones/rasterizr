using SlimShader.ObjectModel.Tokens;

namespace SlimShader.Parser.Declarations
{
	public class ResourceDeclarationParser
	{
		private readonly DxbcReader _reader;

		public ResourceDeclarationParser(DxbcReader reader)
		{
			_reader = reader;
		}

		public DeclareResourceToken Parse()
		{
			var resourceDimension = Decoder.DecodeResourceDimension(_reader.CurrentToken);
			var sampleCount = Decoder.DecodeResourceSampleCount(_reader.CurrentToken);
			_reader.MoveNext();
			return new DeclareResourceToken
			{
				ResourceDimension = resourceDimension,
				SampleCount = sampleCount,
				Operand = new OperandParser(_reader).Parse(),
				ReturnType = ReadResourceReturnType(),
			};
		}

		private ResourceReturnTypeToken ReadResourceReturnType()
		{
			var token = _reader.ReadAndMoveNext();
			return new ResourceReturnTypeToken
			{
				X = Decoder.DecodeResourceReturnType(token, 0),
				Y = Decoder.DecodeResourceReturnType(token, 1),
				Z = Decoder.DecodeResourceReturnType(token, 2),
				W = Decoder.DecodeResourceReturnType(token, 3)
			};
		} 
	}
}