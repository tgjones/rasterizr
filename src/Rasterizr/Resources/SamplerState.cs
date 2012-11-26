namespace Rasterizr.Resources
{
	public class SamplerState : DeviceChild
	{
		private readonly SamplerStateDescription _description;

		public SamplerStateDescription Description
		{
			get { return _description; }
		}

		public SamplerState(Device device, SamplerStateDescription description)
			: base(device)
		{
			_description = description;
		}
	}
}