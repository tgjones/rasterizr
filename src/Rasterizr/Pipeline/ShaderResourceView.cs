using Rasterizr.Resources;

namespace Rasterizr.Pipeline
{
	public class ShaderResourceView : ResourceView
	{
		internal ShaderResourceView(Device device, Resource resource)
			: base(device, resource)
		{
		}
	}
}