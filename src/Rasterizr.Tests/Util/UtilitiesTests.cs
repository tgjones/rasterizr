using System;
using NUnit.Framework;
using Rasterizr.Util;

namespace Rasterizr.Tests.Util
{
	[TestFixture]
	public class UtilitiesTests
	{
		private struct TestStruct
		{
			public int A;
			public float B;
		}

		[Test]
		public void TestSizeOf()
		{
			Assert.That(Utilities.SizeOf<TestStruct>(), Is.EqualTo(8));
		}

		[Test]
		public void CanCopyStructToByteArray()
		{
			// Arrange.
			var structArray = new[]
			{
				new TestStruct
				{
					A = 2,
					B = 3.0f
				}
			};

			// Act.
			var byteArray = new byte[Utilities.SizeOf<TestStruct>()];
			Utilities.ToByteArray(structArray, byteArray, 0);

			// Assert.
			var expectedByteArray = TestHelper.MergeByteArrays(
				BitConverter.GetBytes(2),
				BitConverter.GetBytes(3.0f));
			Assert.That(byteArray, Is.EqualTo(expectedByteArray));
		}

		[Test]
		public void CanCopyByteArrayToStructArray()
		{
			// Arrange.
			var structArray = new[]
			{
				new TestStruct
				{
					A = 2,
					B = 3.0f
				},
				new TestStruct
				{
					A = 4,
					B = 5.5f
				}
			};
			var byteArray = new byte[Utilities.SizeOf<TestStruct>()];
			Utilities.ToByteArray(structArray, byteArray, 0);

			// Act.
			var result = new TestStruct[2];
			Utilities.FromByteArray(result, 0, byteArray, 0, 2 * Utilities.SizeOf<TestStruct>());

			// Assert.
			Assert.That(result, Is.EqualTo(structArray));
		}
	}
}