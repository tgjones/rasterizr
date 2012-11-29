namespace Rasterizr.Pipeline.OutputMerger
{
	public struct DepthStencilOperationDescription
	{
		public StencilOperation FailOperation;
		public StencilOperation DepthFailOperation;
		public StencilOperation PassOperation;
		public Comparison Comparison;
	}
}