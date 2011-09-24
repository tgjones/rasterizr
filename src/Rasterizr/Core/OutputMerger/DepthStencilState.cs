namespace Rasterizr.Core.OutputMerger
{
	public class DepthStencilState
	{
		#region Static stuff

		public static readonly DepthStencilState Default;
		public static readonly DepthStencilState DepthRead;
		public static readonly DepthStencilState None;

		static DepthStencilState()
		{
			None = new DepthStencilState(false, false);
			Default = new DepthStencilState(true, true);
			DepthRead = new DepthStencilState(true, false);
		}

		#endregion

		public bool DepthEnable { get; set; }
		public bool DepthWriteEnable { get; set; }
		public ComparisonFunc DepthFunc { get; set; }

		public DepthStencilState()
		{
			DepthEnable = true;
			DepthWriteEnable = true;
			DepthFunc = ComparisonFunc.LessEqual;
		}

		private DepthStencilState(bool depthEnable, bool depthWriteEnable)
			: this()
		{
			DepthEnable = depthEnable;
			DepthWriteEnable = depthWriteEnable;
		}

		public bool DepthTestPasses(float newDepth, float currentDepth)
		{
			if (!DepthEnable)
				return true;

			return ComparisonUtility.DoComparison(DepthFunc,
				newDepth, currentDepth);
		}
	}
}