using Rasterizr.Util;

namespace Rasterizr.Resources
{
	public abstract class Resource : DeviceChild
	{
		private readonly byte[] _data;

		protected Resource(Device device, int sizeInBytes)
			: base(device)
		{
			_data = new byte[sizeInBytes];
		}

		public void GetData<T>(int dataOffset, T[] data, int startIndex, int countInBytes)
			where T : struct
		{
			Utilities.FromByteArray(data, dataOffset, _data, startIndex, countInBytes);
		}

		public void GetData<T>(T[] data, int startIndex, int countInBytes)
			where T : struct
		{
			GetData(0, data, startIndex, countInBytes);
		}

		public void GetData<T>(out T data, int startIndex, int countInBytes)
			where T : struct
		{
			Utilities.FromByteArray(out data, _data, startIndex, countInBytes);
		}

		public void GetData<T>(T[] data)
			where T : struct
		{
			GetData(data, 0, _data.Length * Utilities.SizeOf<T>());
		}

		public void SetData<T>(T[] data, int startIndex)
			where T : struct
		{
			Utilities.ToByteArray(data, _data, startIndex);
		}

		public void SetData<T>(ref T data, int startIndex)
			where T : struct
		{
			Utilities.ToByteArray(ref data, _data, startIndex);
		}

		public void SetData<T>(T[] data)
			where T : struct
		{
			SetData(data, 0);
		}

		public void SetData<T>(ref T data)
			where T : struct
		{
			Utilities.ToByteArray(ref data, _data, 0);
		}
	}
}