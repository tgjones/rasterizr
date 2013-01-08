namespace Rasterizr.Resources
{
	public abstract class Resource : DeviceChild
	{
		internal interface ISubresource
		{
			void Clear<T>(ref T value)
				where T : struct;
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

		internal abstract MappedSubresource Map(int subresource);
		internal abstract void UpdateSubresource(int subresource, byte[] data);
	}
}