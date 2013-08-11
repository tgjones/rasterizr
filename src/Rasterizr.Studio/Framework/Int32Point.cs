using System.Runtime.InteropServices;

namespace Rasterizr.Studio.Framework
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Int32Point
	{
		public int X;
		public int Y;

		public Int32Point(int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}