namespace Rasterizr.PipelineStages.ShaderStages.Core
{
	public interface ITextureLoader
	{
		ITextureImage LoadTextureFromFile(string uri);
	}
}