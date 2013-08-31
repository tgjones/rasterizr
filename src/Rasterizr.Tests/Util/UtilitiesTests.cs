using System;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Rasterizr.Util;

namespace Rasterizr.Tests.Util
{
	[TestFixture]
	public class UtilitiesTests
	{
        [StructLayout(LayoutKind.Sequential, Pack=1)]
	    private struct TestStruct
	    {
	        public int A;
	        public float B;

	        public override string ToString()
	        {
	            return string.Format("{{A:{0} B:{1}}}", A, B);
	        }
	    }

	    [Test]
		public void TestSizeOf()
		{
			Assert.That(Utilities.SizeOf<TestStruct>(), Is.EqualTo(8));
		}

		[Test]
		public void CanCopyStructArrayToByteArray()
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

			// Act.
			var byteArray = new byte[2 * Utilities.SizeOf<TestStruct>()];
			Utilities.ToByteArray(structArray, byteArray, 0);

			// Assert.
			var expectedByteArray = TestHelper.MergeByteArrays(
				BitConverter.GetBytes(2),
				BitConverter.GetBytes(3.0f),
                BitConverter.GetBytes(4),
                BitConverter.GetBytes(5.5f));
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
			var byteArray = new byte[2 * Utilities.SizeOf<TestStruct>()];
			Utilities.ToByteArray(structArray, byteArray, 0);

			// Act.
			var result = new TestStruct[2];
			Utilities.FromByteArray(result, 0, byteArray, 0, byteArray.Length);

			// Assert.
			Assert.That(result, Is.EqualTo(structArray));
		}

        [Test]
        public void CanCopyByteArrayToStructArrayWithOffset()
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
            var byteArray = new byte[2 * Utilities.SizeOf<TestStruct>()];
            Utilities.ToByteArray(structArray, byteArray, 0);

            // Act.
            var result = new TestStruct[1];
            Utilities.FromByteArray(result, 0, byteArray, Utilities.SizeOf<TestStruct>(), Utilities.SizeOf<TestStruct>());

            // Assert.
            Assert.That(result, Is.EqualTo(new[]
            {
                new TestStruct
                {
                    A = 4,
                    B = 5.5f
                }
            }));
        }
	}
}