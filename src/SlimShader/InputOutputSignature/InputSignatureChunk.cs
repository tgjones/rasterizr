namespace SlimShader.InputOutputSignature
{
	public class InputSignatureChunk : InputOutputSignatureChunk
	{
		public override string ToString()
		{
			return @"// Input signature:
//
" + base.ToString();
		}
	}
}