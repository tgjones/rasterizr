namespace Rasterizr.Core.Diagnostics
{
	public static class GraphicsTrace
	{
		public static readonly GraphicsTraceListenerCollection _listeners = new GraphicsTraceListenerCollection();
		public static GraphicsTraceListenerCollection Listeners
		{
			get { return _listeners; }
		}

		public static void BeginApiCall(string methodName, params object[] methodArguments)
		{
			foreach (var listener in _listeners)
				listener.BeginApiCall(methodName, methodArguments);
		}

		public static void EndApiCall()
		{
			foreach (var listener in _listeners)
				listener.EndApiCall();
		}

		public static void Close()
		{
			foreach (var listener in _listeners)
				listener.Close();
		}
	}
}