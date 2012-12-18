namespace Rasterizr.Diagnostics
{
	public abstract class PixelEvent
	{
		public abstract bool Matches(int x, int y);
	}
}