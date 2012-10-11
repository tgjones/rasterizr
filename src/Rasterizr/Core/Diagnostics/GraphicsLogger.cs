namespace Rasterizr.Core.Diagnostics
{
	public abstract class GraphicsLogger
	{
		protected internal abstract void BeginOperation(OperationType type, params object[] methodArguments);
	}
}