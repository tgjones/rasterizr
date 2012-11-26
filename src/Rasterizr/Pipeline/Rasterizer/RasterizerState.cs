namespace Rasterizr.Pipeline.Rasterizer
{
	public class RasterizerState : DeviceChild
	{
		private readonly RasterizerStateDescription _description;

		public RasterizerStateDescription Description
		{
			get { return _description; }
		}

		public RasterizerState(Device device, RasterizerStateDescription description)
			: base(device)
		{
			_description = description;
		}
	}
}