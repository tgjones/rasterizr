namespace Rasterizr
{
	public abstract class DeviceChild
	{
		private readonly Device _device;
		private readonly int _id;

		public Device Device
		{
			get { return _device; }
		}

		public int ID
		{
			get { return _id; }
		}

		protected DeviceChild(Device device)
		{
			_device = device;
			_id = device.RegisterDeviceChild(this);
		}
	}
}