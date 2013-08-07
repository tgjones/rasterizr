using Rasterizr.Math;

namespace Rasterizr.Resources
{
	public abstract class Resource : DeviceChild
	{
		internal interface ISubresource
		{
		    void Clear(ref Color4F value);
		}

		public static int CalculateSubresource(int mipSlice, int arraySlice, int mipLevels)
		{
			return mipSlice + (arraySlice * mipLevels);
		}

		internal abstract ResourceType ResourceType { get; }

		protected Resource(Device device)
			: base(device)
		{
			
		}
	}
}