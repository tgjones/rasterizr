namespace Rasterizr.Core.Diagnostics
{
	public abstract class GraphicsLogger
	{
		protected internal abstract void BeginApiCall(string methodName, params object[] methodArguments);
		protected internal abstract void EndFrame();
	}
}