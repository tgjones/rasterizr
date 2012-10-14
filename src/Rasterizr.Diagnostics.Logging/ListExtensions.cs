using System.Collections.Generic;

namespace Rasterizr.Diagnostics.Logging
{
	public static class ListExtensions
	{
		public static T Get<T>(this List<object> list, int index)
		{
			return (T) list[index];
		}
	}
}