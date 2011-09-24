using System;

namespace Rasterizr.Core.OutputMerger
{
	public static class ComparisonUtility
	{
		public static bool DoComparison(ComparisonFunc func, float source, float dest)
		{
			switch (func)
			{
				case ComparisonFunc.Never:
					return false;
				case ComparisonFunc.Less:
					return (source < dest);
				case ComparisonFunc.Equal :
					return (source == dest);
				case ComparisonFunc.LessEqual :
					return (source <= dest);
				case ComparisonFunc.Greater :
					return (source > dest);
				case ComparisonFunc.NotEqual :
					return (source != dest);
				case ComparisonFunc.GreaterEqual :
					return (source >= dest);
				case ComparisonFunc.Always :
					return true;
				default :
					throw new NotSupportedException();
			}
		}
	}
}