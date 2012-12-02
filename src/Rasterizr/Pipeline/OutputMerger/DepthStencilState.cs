namespace Rasterizr.Pipeline.OutputMerger
{
	public class DepthStencilState : DeviceChild
	{
		private readonly DepthStencilStateDescription _description;

		public DepthStencilStateDescription Description
		{
			get { return _description; }
		}

		internal DepthStencilState(Device device, DepthStencilStateDescription description)
			: base(device)
		{
			_description = description;
		}

		internal bool DepthTestPasses(float newDepth, float currentDepth)
		{
			if (!Description.IsDepthEnabled)
				return true;

			return ComparisonUtility.DoComparison(Description.DepthComparison,
				newDepth, currentDepth);
		}
	}
}