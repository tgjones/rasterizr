namespace Rasterizr.Resources
{
	public struct BufferDescription
	{
		public int SizeInBytes;
		public BindFlags BindFlags;

		public BufferDescription(BindFlags bindFlags)
		{
			SizeInBytes = 0;
			BindFlags = bindFlags;
		}
	}
}