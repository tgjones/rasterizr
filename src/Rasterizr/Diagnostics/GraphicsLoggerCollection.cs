using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Rasterizr.Diagnostics
{
	public class GraphicsLoggerCollection : ReadOnlyCollection<GraphicsLogger>
	{
	    private readonly bool _enabled;

		public GraphicsLoggerCollection(IEnumerable<GraphicsLogger> list)
			: base(list.ToList())
		{
		    _enabled = this.Any();
		}

		internal void BeginOperation(OperationType type, params object[] methodArguments)
		{
            if (!_enabled)
                return;
			foreach (var logger in this)
				logger.BeginOperation(type, methodArguments);
		}

		internal void AddPixelHistoryEvent(PixelEvent @event)
		{
            if (!_enabled)
                return;
			foreach (var logger in this)
				logger.AddPixelEvent(@event);
		}
	}
}