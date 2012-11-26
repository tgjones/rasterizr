namespace Rasterizr.Pipeline.OutputMerger
{
	public class BlendState : DeviceChild
	{
		private readonly BlendStateDescription _description;

		public BlendStateDescription Description
		{
			get { return _description; }
		}
		
		public BlendState(Device device, BlendStateDescription description)
			: base(device)
		{
			_description = description;
		}
	}
}