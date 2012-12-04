using Rasterizr.Diagnostics;
using Rasterizr.Util;

namespace Rasterizr.Resources
{
	public class Buffer : Resource
	{
		internal Buffer(Device device, BufferDescription description)
			: base(device, description.SizeInBytes)
		{
			
		}
	}
}