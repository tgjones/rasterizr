using NUnit.Framework;
using Nexus;
using Rasterizr.Resources;
using Rasterizr.Util;

namespace Rasterizr.Tests.Resources
{
	[TestFixture]
	public class BufferTests
	{
		[Test]
		public void CanSetAndGetMatrix()
		{
			// Arrange.
			var device = new Device();
			var description = new BufferDescription
			{
				SizeInBytes = Utilities.SizeOf<Matrix3D>()
			};
			var buffer = new Buffer(device, description);
			var matrix = Matrix3D.CreateLookAt(new Point3D(1, 2, 3), Vector3D.Forward, Vector3D.Up);

			// Act.
			buffer.SetData(ref matrix);

			Matrix3D retrievedMatrix;
			buffer.GetData(out retrievedMatrix, 0, Utilities.SizeOf<Matrix3D>());

			// Assert.
			Assert.That(retrievedMatrix, Is.EqualTo(matrix));
		}
	}
}