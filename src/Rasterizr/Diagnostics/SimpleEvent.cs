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
	}
}