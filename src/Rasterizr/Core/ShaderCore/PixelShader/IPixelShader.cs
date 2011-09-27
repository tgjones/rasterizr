using System.Threading;
using Nexus;
using Rasterizr.Core.Rasterizer;

namespace Rasterizr.Core.ShaderCore.PixelShader
{
	public interface IPixelShader : IShader
	{
		ThreadLocal<FragmentQuadLocation> FragmentQuadLocation { get; }

		Vector2D Ddx(Point2D x);
		Vector2D Ddy(Point2D x);
	}
}