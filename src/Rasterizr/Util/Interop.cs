using System;

namespace Rasterizr.Util
{
	internal static class Interop
	{
		public static unsafe void Read<T>(void* pSrc, T[] data, int offset, int countInBytes)
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

		public static unsafe void Write<T>(void* pDest, T[] data, int offset, int count)
			where T : struct
		{
			throw new NotImplementedException();
		}
	}
}