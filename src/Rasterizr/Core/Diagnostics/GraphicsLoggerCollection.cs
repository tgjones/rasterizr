using System.Collections.ObjectModel;

namespace Rasterizr.Core.Diagnostics
{
	public class GraphicsLoggerCollection : Collection<GraphicsLogger>
	{
		internal void BeginApiCall(string methodName, params object[] methodArguments)
		{
			foreach (var logger in this)
				logger.BeginApiCall(methodName, methodArguments);
		}

		internal void EndFrame()
		{
			foreach (var logger in this)
				logger.EndFrame();
		}
	}
}