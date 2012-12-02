using Rasterizr.Diagnostics;
using Rasterizr.Util;

namespace Rasterizr.Resources
{
	public class Buffer : Resource
	{
		public static Buffer Create<T>(Device device, BindFlags bindFlags, T[] data, int sizeInBytes = 0)
			where T : struct
		{
			var description = new BufferDescription
			{
				BindFlags = bindFlags,
				SizeInBytes = sizeInBytes == 0 ? Utilities.SizeOf<T>() * data.Length : sizeInBytes
			};

			var buffer = new Buffer(device, description);
			buffer.SetData(data);
			return buffer;
		}

		public Buffer(Device device, BufferDescription description)
			: base(device, description.SizeInBytes)
		{
			device.Loggers.BeginOperation(OperationType.BufferCreate, description);
		}
	}
}