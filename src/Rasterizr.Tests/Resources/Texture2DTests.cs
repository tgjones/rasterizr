using NUnit.Framework;
using Rasterizr.Resources;

namespace Rasterizr.Tests.Resources
{
	[TestFixture]
	public class Texture2DTests
	{
		[Test]
		public void TextureHasCorrectNumberOfMipMapLevels()
		{
			// Arrange.
			var texture = new Texture2D(new Device(), new Texture2DDescription
			{
				Width = 32,
				Height = 32,
				ArraySize = 1,
				Format = Format.R8G8B8A8_UInt
			});
			int width, height, numberOfLevels;

			// Act.
			texture.GetDimensions(0, out width, out height, out numberOfLevels);

			// Assert.
			Assert.That(width, Is.EqualTo(32));
			Assert.That(height, Is.EqualTo(32));
			Assert.That(numberOfLevels, Is.EqualTo(6));
		}

		[TestCase(0, 64, 32)]
		[TestCase(1, 32, 16)]
		[TestCase(2, 16, 8)]
		[TestCase(3, 8, 4)]
		[TestCase(4, 4, 2)]
		[TestCase(5, 2, 1)]
		[TestCase(6, 1, 1)]
		public void MipMapsHaveCorrectDimensions(int mipLevel, int expectedWidth, int expectedHeight)
		{
			// Arrange.
			var texture = new Texture2D(new Device(), new Texture2DDescription
			{
				Width = 64,
				Height = 32,
				ArraySize = 1,
				Format = Format.R8G8B8A8_UInt
			});

			// Act / Assert.
			int actualWidth, actualHeight;
			texture.GetDimensions(mipLevel, out actualWidth, out actualHeight);
			Assert.That(actualWidth, Is.EqualTo(expectedWidth));
			Assert.That(actualHeight, Is.EqualTo(expectedHeight));
		}
	}
}