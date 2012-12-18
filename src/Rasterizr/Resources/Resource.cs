using Rasterizr.Util;

namespace Rasterizr.Resources
{
	public abstract class Resource : DeviceChild
	{
		private readonly int _sizeInBytes;
		private readonly int _strideInBytes;
		private readonly byte[] _data;

		public int SizeInBytes
		{
			get { return _sizeInBytes; }
		}

		internal abstract ResourceType ResourceType { get; }
		internal abstract int NumElements { get; }

		protected Resource(Device device, int sizeInBytes, int strideInBytes)
			: base(device)
		{
			_sizeInBytes = sizeInBytes;
			_strideInBytes = strideInBytes;
			_data = new byte[sizeInBytes];
		}

		internal abstract int CalculateByteOffset(int x, int y, int z);

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

		public void Fill<T>(ref T value)
			where T : struct
		{
			var offset = 0;
			while (offset < _sizeInBytes)
			{
				SetData(ref value, offset);
				offset += _strideInBytes;
			}
		}
	}
}