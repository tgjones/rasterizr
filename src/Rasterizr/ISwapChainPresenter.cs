using SlimShader;

namespace Rasterizr
{
    public interface ISwapChainPresenter
    {
        void Initialize(int width, int height);
        void Present(Number4[] colors);
    }
}