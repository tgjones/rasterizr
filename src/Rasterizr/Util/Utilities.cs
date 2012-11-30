namespace Rasterizr.Util
{
	public static class Utilities
	{
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

		public static void ToByteArray<T>(ref T source, byte[] destination, int destinationOffset)
			where T : struct
		{
			unsafe
			{
				fixed (byte* pBuffer = &destination[destinationOffset])
					Interop.Write(pBuffer, ref source);
			}
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

		/// <summary>
		/// Return the sizeof an array of struct. Equivalent to sizeof operator but works on generics too.
		/// </summary>
		/// <typeparam name="T">a struct</typeparam>
		/// <param name="array">The array of struct to evaluate.</param>
		/// <returns>sizeof in bytes of this array of struct</returns>
		public static int SizeOf<T>(T[] array) where T : struct
		{
			return array == null ? 0 : array.Length * Interop.SizeOf<T>();
		}
	}
}