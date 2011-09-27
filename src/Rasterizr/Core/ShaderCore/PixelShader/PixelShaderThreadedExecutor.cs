using System.Threading;
using Nexus;
using Rasterizr.Core.Rasterizer;

namespace Rasterizr.Core.ShaderCore.PixelShader
{
	public class PixelShaderThreadedExecutor
	{
		private readonly CountdownEvent _countdownEvent;

		public PixelShaderThreadedExecutor()
		{
			_countdownEvent = new CountdownEvent(4);
		}

		public Pixel[] Execute(IPixelShader pixelShader, FragmentQuad fragmentQuad)
		{
			_countdownEvent.Reset();
			var pixels = new Pixel[4];
			for (int i = 0; i < fragmentQuad.Fragments.Length; ++i)
				ThreadPool.QueueUserWorkItem(state =>
				{
					var index = (int) state;
					var fragment = fragmentQuad.Fragments[index];
					pixelShader.FragmentQuadLocation.Value = fragment.QuadLocation;
					var color = (ColorF) pixelShader.Execute(fragment.PixelShaderInput);
					pixels[index] = new Pixel(fragment.X, fragment.Y)
					{
						Color = color,
						Depth = fragment.Depth,
						Samples = fragment.Samples
					};
					_countdownEvent.Signal();
				}, i);
			_countdownEvent.Wait();
			return pixels;
		}
	}
}