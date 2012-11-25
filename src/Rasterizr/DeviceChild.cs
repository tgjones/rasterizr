namespace Rasterizr
{
	public abstract class DeviceChild
	{
		private readonly Device _device;

		public Device Device
		{
			get { return _device; }
		}

		protected DeviceChild(Device device)
		{
			_device = device;
		}
	}
}