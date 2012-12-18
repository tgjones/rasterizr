namespace Rasterizr.Resources
{
	public class Buffer : Resource
	{
		internal override ResourceType ResourceType
		{
			get { return ResourceType.Buffer; }
		}

		internal override int NumElements
		{
			get { return 1; }
		}

		internal Buffer(Device device, BufferDescription description)
			: base(device, description.SizeInBytes, 0)
		{
			
		}

		internal override int CalculateByteOffset(int x, int y, int z)
		{
			return 0;
		}
	}
}