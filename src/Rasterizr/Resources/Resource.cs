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

		internal byte[] Data
		{
			get { return _data; }
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

			// TODO: This might be faster.
			//var typedColor = color.ToColor4();
			//var invertedColor = new Color4(typedColor.B, typedColor.G, typedColor.R, typedColor.A);
			//var texture = (Texture2D)Resource;

			//// Fill first line.
			//int width = texture.Description.Width;
			//for (int x = 0; x < width; x++)
			//	_colors[x] = invertedColor;

			//// Copy first line.
			//int height = texture.Description.Height;
			//int sizeToCopy = Utilities.SizeOf<Color4>() * width;
			//fixed (Color4* src = &_colors[0])
			//	for (int y = 1; y < height; y++)
			//	{
			//		var dest = (void*)((IntPtr)src + y * sizeToCopy);
			//		Interop.memcpy(dest, src, sizeToCopy);
			//	}

			//Resource.SetData(_colors);
		}
	}
}