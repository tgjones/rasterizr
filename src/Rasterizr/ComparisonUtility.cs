using System;

namespace Rasterizr
{
	internal static class ComparisonUtility
	{
		public static bool DoComparison(Comparison func, float source, float dest)
		{
			switch (func)
			{
				case Comparison.Never:
					return false;
				case Comparison.Less:
					return (source < dest);
				case Comparison.Equal:
					return (source == dest);
				case Comparison.LessEqual:
					return (source <= dest);
				case Comparison.Greater:
					return (source > dest);
				case Comparison.NotEqual:
					return (source != dest);
				case Comparison.GreaterEqual:
					return (source >= dest);
				case Comparison.Always:
					return true;
				default:
					throw new ArgumentOutOfRangeException("func");
			}
		}
	}
}