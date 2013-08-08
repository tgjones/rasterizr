namespace Rasterizr.Pipeline.OutputMerger
{
	public class DepthStencilState : DeviceChild
	{
		private readonly DepthStencilStateDescription _description;
	    private readonly bool _isDepthEnabled;
	    private readonly Comparison _depthComparison;

		public DepthStencilStateDescription Description
		{
			get { return _description; }
		}

		internal DepthStencilState(Device device, DepthStencilStateDescription description)
			: base(device)
		{
			_description = description;
		    _isDepthEnabled = description.IsDepthEnabled;
		    _depthComparison = description.DepthComparison;
		}

		internal bool DepthTestPasses(float newDepth, float currentDepth)
		{
			if (!_isDepthEnabled)
				return true;

			return ComparisonUtility.DoComparison(
                _depthComparison,
				newDepth, currentDepth);
		}
	}
}