using System.Threading;
using Nexus;
using Rasterizr.Core.Rasterizer;
using Rasterizr.Core.Rasterizer.Derivatives;

namespace Rasterizr.Core.ShaderCore.PixelShader
{
	public abstract class PixelShaderBase<TPixelShaderInput> : ShaderBase<TPixelShaderInput, ColorF>, IPixelShader
		where TPixelShaderInput : new()
	{
		private readonly ThreadLocal<FragmentQuadLocation> _fragmentQuadLocation;
		private readonly Point2DDerivativeValues _derivatives;
		private readonly Barrier _barrier;

		public ThreadLocal<FragmentQuadLocation> FragmentQuadLocation
		{
			get { return _fragmentQuadLocation; }
		}

		protected PixelShaderBase()
		{
			_fragmentQuadLocation = new ThreadLocal<FragmentQuadLocation>();
			_derivatives = new Point2DDerivativeValues();
			_barrier = new Barrier(4);
		}

		public Vector2D Ddx(Point2D x)
		{
			// Store value for this thread and wait for other threads to catch up.
			_derivatives[_fragmentQuadLocation.Value] = x;
			_barrier.SignalAndWait();

			// Make sure all threads get the correct value before we move on.
			var result = _derivatives.Ddx(_fragmentQuadLocation.Value);
			_barrier.SignalAndWait();

			return result;
		}

		public Vector2D Ddy(Point2D x)
		{
			// Store value for this thread and wait for other threads to catch up.
			_derivatives[_fragmentQuadLocation.Value] = x;
			_barrier.SignalAndWait();

			// Make sure all threads get the correct value before we move on.
			var result = _derivatives.Ddy(_fragmentQuadLocation.Value);
			_barrier.SignalAndWait();

			return result;
		}
	}
}