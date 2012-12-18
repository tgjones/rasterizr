using Rasterizr.Math;

namespace Rasterizr.Diagnostics
{
	public class ClearRenderTargetEvent : PixelEvent
	{
		public Color4F Result { get; private set; }

		public ClearRenderTargetEvent(Color4F result)
		{
			Result = result;
		}

		public override bool Matches(int x, int y)
		{
			return true;
		}
	}
}