using System.Collections.ObjectModel;

namespace Rasterizr.Core.Diagnostics
{
	public class GraphicsLoggerCollection : Collection<GraphicsLogger>
	{
		internal void BeginOperation(OperationType type, params object[] methodArguments)
		{
			foreach (var logger in this)
				logger.BeginOperation(type, methodArguments);
		}
	}
}