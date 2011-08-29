namespace Rasterizr.ShaderStages.Core
{
	public interface ITextureLoader
	{
		ITextureImage LoadTextureFromFile(string uri);
	}
}