using SlimShader;

namespace Rasterizr.Diagnostics
{
	public class ClearRenderTargetEvent : PixelEvent
	{
        public Number4 Result { get; private set; }

        public ClearRenderTargetEvent(Number4 result)
		{
			Result = result;
		}

		public override bool Matches(int x, int y)
		{
			return true;
		}
	}
}