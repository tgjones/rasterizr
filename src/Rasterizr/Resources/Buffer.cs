using Rasterizr.Diagnostics;
using Rasterizr.Util;

namespace Rasterizr.Resources
{
	public class Buffer : Resource
	{
		private readonly int _sizeInBytes;
		private readonly byte[] _data;

		internal override ResourceType ResourceType
		{
			get { return ResourceType.Buffer; }
		}

		internal byte[] Data
		{
			get { return _data; }
		}

		internal Buffer(Device device, BufferDescription description)
			: base(device)
		{
			_sizeInBytes = description.SizeInBytes;
			_data = new byte[_sizeInBytes];
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

        //public void SetData<T>(T[] data, int startIndex = 0)
        //    where T : struct
        //{
        //    Utilities.ToByteArray(data, _data, startIndex);
        //}

		public void SetData<T>(ref T data, int offsetInBytes = 0)
			where T : struct
		{
            SetData(Utilities.ToByteArray(ref data), offsetInBytes);
		}

	    public void SetData(byte[] data, int offsetInBytes = 0)
	    {
            Device.Loggers.BeginOperation(OperationType.BufferSetData, ID, data, offsetInBytes);
	        Utilities.ToByteArray(data, _data, offsetInBytes);
	    }
	}
}