using NUnit.Framework;
using Rasterizr.Resources;

namespace Rasterizr.Tests.Resources
{
	[TestFixture]
	public class Texture3DTests
	{
		[Test]
		public void TextureHasCorrectNumberOfMipMapLevels()
		{
			// Arrange.
			var texture = new Texture3D(new Device(), new Texture3DDescription
			{
				Width = 32,
				Height = 32,
				Depth = 32
			});
			int width, height, depth, numberOfLevels;

			// Act.
			texture.GetDimensions(0, out width, out height, out depth, out numberOfLevels);

			// Assert.
			Assert.That(width, Is.EqualTo(32));
			Assert.That(height, Is.EqualTo(32));
			Assert.That(depth, Is.EqualTo(32));
			Assert.That(numberOfLevels, Is.EqualTo(6));
		}

		[TestCase(0, 64, 32, 16)]
		[TestCase(1, 32, 16, 8)]
		[TestCase(2, 16, 8, 4)]
		[TestCase(3, 8, 4, 2)]
		[TestCase(4, 4, 2, 1)]
		[TestCase(5, 2, 1, 1)]
		[TestCase(6, 1, 1, 1)]
		public void MipMapsHaveCorrectDimensions(int mipLevel, int expectedWidth, int expectedHeight, int expectedDepth)
		{
			// Arrange.
			var texture = new Texture3D(new Device(), new Texture3DDescription
			{
				Width = 64,
				Height = 32,
				Depth = 16
			});

			// Act / Assert.
			int actualWidth, actualHeight, actualDepth;
			texture.GetDimensions(mipLevel, out actualWidth, out actualHeight, out actualDepth);
			Assert.That(actualWidth, Is.EqualTo(expectedWidth));
			Assert.That(actualHeight, Is.EqualTo(expectedHeight));
			Assert.That(actualDepth, Is.EqualTo(expectedDepth));
		}
	}
}