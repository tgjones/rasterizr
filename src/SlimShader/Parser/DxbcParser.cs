namespace SlimShader.Parser
{
	public abstract class DxbcParser
	{
		protected static uint GetFourCc(char a, char b, char c, char d)
		{
			return a | ((uint) (b << 8)) | ((uint) c << 16) | ((uint) d << 24);
		}
	}
}