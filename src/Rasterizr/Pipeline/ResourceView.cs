using Rasterizr.Resources;

namespace Rasterizr.Pipeline
{
	public abstract class ResourceView : DeviceChild
	{
		private readonly Resource _resource;

		public Resource Resource
		{
			get { return _resource; }
		}

		internal ResourceView(Device device, Resource resource)
			: base(device)
		{
			_resource = resource;
		}
	}
}