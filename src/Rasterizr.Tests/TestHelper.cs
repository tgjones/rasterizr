using System;
using System.Collections.Generic;
using System.Linq;

namespace Rasterizr.Tests
{
	internal static class TestHelper
	{
		public static byte[] GetByteArray(params float[] values)
		{
			return values.SelectMany(BitConverter.GetBytes).ToArray();
		}

		public static byte[] MergeByteArrays(params byte[][] byteArrays)
		{
			var result = new List<byte>();
			foreach (var byteArray in byteArrays)
				result.AddRange(byteArray);
			return result.ToArray();
		}
	}
}