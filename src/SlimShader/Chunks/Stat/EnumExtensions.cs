using SlimShader.Shader;

namespace SlimShader.Chunks.Stat
{
	public static class EnumExtensions
	{
		public static string GetDescription(this TessellatorDomain value)
		{
			return value.ToString();
		}
	}
}