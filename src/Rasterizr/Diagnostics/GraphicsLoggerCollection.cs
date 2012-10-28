using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rasterizr.Diagnostics
{
	public class GraphicsLoggerCollection : ReadOnlyCollection<GraphicsLogger>
	{
		public GraphicsLoggerCollection(IList<GraphicsLogger> list)
			: base(list)
		{
		}

		internal void BeginOperation(OperationType type, params object[] methodArguments)
		{
			foreach (var logger in this)
				logger.BeginOperation(type, methodArguments);
		}
	}
}