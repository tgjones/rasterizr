using Nexus;

namespace Rasterizr.ShaderStages.Core
{
	public interface ITextureImage
	{
		int Dimensions { get; }
		int GetBound(int dimension);

		ColorF GetColor(int x, int y);
	}
}