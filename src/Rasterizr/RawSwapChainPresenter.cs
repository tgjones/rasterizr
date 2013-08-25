using SlimShader;

namespace Rasterizr
{
	public class RawSwapChainPresenter : ISwapChainPresenter
	{
	    public Number4[] Data { get; private set; }

	    void ISwapChainPresenter.Initialize(int width, int height) { }

        void ISwapChainPresenter.Present(Number4[] colors)
		{
			Data = colors;
		}
	}
}