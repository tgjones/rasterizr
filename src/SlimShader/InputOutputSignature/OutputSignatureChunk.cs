namespace SlimShader.InputOutputSignature
{
	public class OutputSignatureChunk : InputOutputSignatureChunk
	{
		public override string ToString()
		{
			return @"// Output signature:
//
" + base.ToString();
		}
	}
}