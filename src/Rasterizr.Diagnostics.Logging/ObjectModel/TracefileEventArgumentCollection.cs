using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rasterizr.Diagnostics.Logging.ObjectModel
{
	public class TracefileEventArgumentCollection : Collection<object>
	{
		public TracefileEventArgumentCollection(IList<object> list)
			: base(list)
		{
			
		}

		public TracefileEventArgumentCollection()
		{
			
		}

		public T Get<T>(int index)
		{
			return (T) this[index];
		}
	}
}