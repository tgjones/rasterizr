using System;

namespace Rasterizr.Util
{
	internal static class Interop
	{
		public static unsafe void* Cast<T>(ref T data)
			where T : struct
		{
			throw new NotImplementedException();
		}

		public static unsafe void CopyInline<T>(void* pDest, ref T srcData)
			where T : struct
		{
			throw new NotImplementedException();
		}

		public static unsafe void* Fixed<T>(ref T data)
		{
			throw new NotImplementedException();
		}

		public static unsafe void* Fixed<T>(T[] data)
		{
			throw new NotImplementedException();
		}

		public static unsafe void memcpy(void* pDest, void* pSrc, int count)
		{
			throw new NotImplementedException();
		}

		public static unsafe void* Read<T>(void* pSrc, ref T data)
			where T : struct
		{
			throw new NotImplementedException();
		}

		public static unsafe void* Read<T>(void* pSrc, T[] data, int offset, int count)
			where T : struct
		{
			throw new NotImplementedException();
		}

		public static int SizeOf<T>()
		{
			throw new NotImplementedException();
		}

		public static unsafe void* Write<T>(void* pDest, ref T data) where T : struct
		{
			throw new NotImplementedException();
		}

		public static unsafe void* Write<T>(void* pDest, T[] data, int offset, int count)
			where T : struct
		{
			throw new NotImplementedException();
		}
	}
}