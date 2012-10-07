namespace Rasterizr.Core.Diagnostics
{
	public abstract class GraphicsTraceListener
	{
		public abstract void BeginApiCall(string methodName, params object[] methodArguments);
		public abstract void EndApiCall();
		public abstract void Close();
	}
}