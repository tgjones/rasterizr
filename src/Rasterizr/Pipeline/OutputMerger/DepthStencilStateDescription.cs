namespace Rasterizr.Pipeline.OutputMerger
{
	public struct DepthStencilStateDescription
	{
		public bool IsDepthEnabled;
		public DepthWriteMask DepthWriteMask;
		public Comparison DepthComparison;
		public bool IsStencilEnabled;
		public byte StencilReadMask;
		public byte StencilWriteMask;
		public DepthStencilOperationDescription FrontFace;
		public DepthStencilOperationDescription BackFace;

		public static DepthStencilStateDescription Default
		{
			get
			{
				return new DepthStencilStateDescription
				{
					IsDepthEnabled = true,
					DepthWriteMask = DepthWriteMask.All,
					DepthComparison = Comparison.Less,
					IsStencilEnabled = false
				};
			}
		}
	}
}