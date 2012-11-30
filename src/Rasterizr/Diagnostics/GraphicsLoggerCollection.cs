using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Rasterizr.Diagnostics
{
	public class GraphicsLoggerCollection : ReadOnlyCollection<GraphicsLogger>
	{
		public GraphicsLoggerCollection(IEnumerable<GraphicsLogger> list)
			: base(list.ToList())
		{
		}

		internal void BeginOperation(OperationType type, params object[] methodArguments)
		{
			foreach (var logger in this)
				logger.BeginOperation(type, methodArguments);
		}
	}
}