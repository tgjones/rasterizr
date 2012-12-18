namespace Rasterizr.Diagnostics
{
	public abstract class GraphicsLogger
	{
		protected internal abstract void BeginOperation(OperationType type, params object[] methodArguments);
		protected internal abstract void AddPixelEvent(PixelEvent @event);
	}
}