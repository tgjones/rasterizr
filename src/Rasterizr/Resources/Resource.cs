using SlimShader;

namespace Rasterizr.Resources
{
	public abstract class Resource : DeviceChild
	{
		internal interface ISubresource
		{
            void Clear(ref Number4 value);
		}

		public static int CalculateSubresource(int mipSlice, int arraySlice, int mipLevels)
		{
			return mipSlice + (arraySlice * mipLevels);
		}

		internal abstract ResourceType ResourceType { get; }
        internal abstract int Size { get; }

		protected Resource(Device device)
			: base(device)
		{
			
		}
	}
}