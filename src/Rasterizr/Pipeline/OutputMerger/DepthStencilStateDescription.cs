namespace Rasterizr.Pipeline.OutputMerger
{
	public struct DepthStencilStateDescription
	{
		public static DepthStencilStateDescription Default
		{
			get { return new DepthStencilStateDescription(true, DepthWriteMask.All); }
		}

		public static DepthStencilStateDescription DepthRead
		{
			get { return new DepthStencilStateDescription(true, DepthWriteMask.Zero); }
		}

		public static DepthStencilStateDescription None
		{
			get { return new DepthStencilStateDescription(false, DepthWriteMask.Zero); }
		}

		public bool IsDepthEnabled;
		public DepthWriteMask DepthWriteMask;
		public Comparison DepthComparison;
		public bool IsStencilEnabled;
		public byte StencilReadMask;
		public byte StencilWriteMask;
		public DepthStencilOperationDescription FrontFace;
		public DepthStencilOperationDescription BackFace;

		public DepthStencilStateDescription(bool isDepthEnabled, DepthWriteMask depthWriteMask)
		{
			IsDepthEnabled = isDepthEnabled;
			DepthWriteMask = depthWriteMask;
			DepthComparison = Comparison.Less;
			IsStencilEnabled = false;
			StencilReadMask = 0;
			StencilWriteMask = 0;
			FrontFace = new DepthStencilOperationDescription();
			BackFace = new DepthStencilOperationDescription();
		}
	}
}