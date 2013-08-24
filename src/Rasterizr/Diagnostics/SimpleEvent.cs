using SlimShader;

namespace Rasterizr.Diagnostics
{
	public class SimpleEvent : PixelEvent
	{
        public Number4 Result { get; private set; }

        public SimpleEvent(Number4 result)
		{
			Result = result;
		}

		public override bool Matches(int x, int y)
		{
			return true;
		}
	}
}