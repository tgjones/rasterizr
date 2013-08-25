using NUnit.Framework;
using Rasterizr.Resources;
using SharpDX;
using Utilities = Rasterizr.Util.Utilities;

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
				SizeInBytes = Utilities.SizeOf<Matrix>()
			};
			var buffer = new Buffer(device, description);
            var matrix = Matrix.LookAtRH(new Vector3(1, 2, 3), Vector3.ForwardRH, Vector3.Up);

			// Act.
            device.ImmediateContext.SetBufferData(buffer, ref matrix);

			Matrix retrievedMatrix;
			buffer.GetData(out retrievedMatrix, 0, Utilities.SizeOf<Matrix>());

			// Assert.
			Assert.That(retrievedMatrix, Is.EqualTo(matrix));
		}
	}
}