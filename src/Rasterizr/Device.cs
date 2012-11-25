namespace Rasterizr
{
	public class Device
	{
		private readonly DeviceContext _immediateContext;

		public DeviceContext ImmediateContext
		{
			get { return _immediateContext; }
		}

		public Device()
		{
			_immediateContext = new DeviceContext();
		}
	}
}