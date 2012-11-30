using Rasterizr.Resources;

namespace Rasterizr.Pipeline
{
	public class UnorderedAccessView : ResourceView
	{
		public UnorderedAccessView(Device device, Resource resource)
			: base(device, resource)
		{
		}
	}
}