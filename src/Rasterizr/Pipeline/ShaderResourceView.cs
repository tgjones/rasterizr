using Rasterizr.Resources;

namespace Rasterizr.Pipeline
{
	public class ShaderResourceView : ResourceView
	{
		public ShaderResourceView(Device device, Resource resource)
			: base(device, resource)
		{
		}
	}
}