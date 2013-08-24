using System;

namespace Rasterizr.Util
{
	public static class Utilities
	{
	    public static void Copy<T1, T2>(T1[] source, T2[] destination)
            where T1 : struct
            where T2 : struct
	    {
            if (SizeOf<T1>() != SizeOf<T2>())
                throw new InvalidOperationException();

	        var destinationBytes = new byte[source.Length * SizeOf<T2>()];
            unsafe
            {
                fixed (byte* pBuffer = &destinationBytes[0])
                    Interop.Write(pBuffer, source, 0, source.Length);
            }
            FromByteArray(destination, 0, destinationBytes, 0, destinationBytes.Length);
	    }

		/// <summary>
		/// Converts a structured array to an equivalent byte array.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="destination"></param>
		/// <param name="destinationOffset"></param>
		/// <returns></returns>
		public static void ToByteArray<T>(T[] source, byte[] destination, int destinationOffset)
			where T : struct
		{
			if (source == null)
				return;

			if (source.Length == 0)
				return;

			unsafe
			{
				fixed (byte* pBuffer = &destination[destinationOffset])
					Interop.Write(pBuffer, source, 0, source.Length);
			}
		}

		public static byte[] ToByteArray<T>(T[] source)
			where T : struct
		{
			if (source == null)
				return null;

			if (source.Length == 0)
				return null;

			var destination = new byte[SizeOf<T>() * source.Length];

			unsafe
			{
				fixed (byte* pBuffer = &destination[0])
					Interop.Write(pBuffer, source, 0, source.Length);
			}

			return destination;
		}

		public static void ToByteArray<T>(ref T source, byte[] destination, int destinationOffset)
			where T : struct
		{
			unsafe
			{
				fixed (byte* pBuffer = &destination[destinationOffset])
					Interop.Write(pBuffer, ref source);
			}
		}

        public static byte[] ToByteArray<T>(ref T source)
            where T : struct
        {
            var destination = new byte[SizeOf<T>()];
            unsafe
            {
                fixed (byte* pBuffer = &destination[0])
                    Interop.Write(pBuffer, ref source);
            }
            return destination;
        }

		public static void FromByteArray<T>(T[] destination, int destinationOffset, byte[] source, int sourceOffset, int countInBytes)
			where T : struct
		{
			unsafe
			{
				fixed (byte* pBuffer = &source[sourceOffset])
					Interop.Read(pBuffer, destination, destinationOffset, countInBytes);
			}
		}

		public static void FromByteArray<T>(out T destination, byte[] source, int sourceOffset, int countInBytes)
			where T : struct
		{
			// TODO: Add Interop.Read that takes a single struct and count in bytes.
			var destinationArray = new T[1];
			unsafe
			{
				fixed (byte* pBuffer = &source[sourceOffset])
					Interop.Read(pBuffer, destinationArray, 0, countInBytes);
			}
			destination = destinationArray[0];
		}

		/// <summary>
		/// Return the sizeof a struct from a CLR. Equivalent to sizeof operator but works on generics too.
		/// </summary>
		/// <typeparam name="T">a struct to evaluate</typeparam>
		/// <returns>sizeof this struct</returns>
		public static int SizeOf<T>() where T : struct
		{
			return Interop.SizeOf<T>();
		}
	}
}