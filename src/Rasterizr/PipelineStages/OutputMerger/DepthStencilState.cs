namespace Rasterizr.PipelineStages.OutputMerger
{
	public class DepthStencilState
	{
		public bool DepthEnable { get; set; }
		public ComparisonFunc DepthFunc { get; set; }

		public DepthStencilState()
		{
			DepthEnable = true;
			DepthFunc = ComparisonFunc.Less;
		}
	}
}