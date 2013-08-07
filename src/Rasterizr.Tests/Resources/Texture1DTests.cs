using NUnit.Framework;
using Rasterizr.Resources;

namespace Rasterizr.Tests.Resources
{
	[TestFixture]
	public class Texture1DTests
	{
		[Test]
		public void TextureHasCorrectNumberOfMipMapLevels()
		{
			// Arrange.
			var texture = new Texture1D(new Device(), new Texture1DDescription
			{
				Width = 32,
				ArraySize = 1
			});
			int width, numberOfLevels;

			// Act.
			texture.GetDimensions(0, out width, out numberOfLevels);

			// Assert.
			Assert.That(width, Is.EqualTo(32));
			Assert.That(numberOfLevels, Is.EqualTo(6));
		}

		[TestCase(0, 64)]
		[TestCase(1, 32)]
		[TestCase(2, 16)]
		[TestCase(3, 8)]
		[TestCase(4, 4)]
		[TestCase(5, 2)]
		[TestCase(6, 1)]
		public void MipMapsHaveCorrectDimensions(int mipLevel, int expectedWidth)
		{
			// Arrange.
			var texture = new Texture1D(new Device(), new Texture1DDescription
			{
				Width = 64,
				ArraySize = 1
			});

			// Act / Assert.
			int actualWidth;
			texture.GetDimensions(mipLevel, out actualWidth);
			Assert.That(actualWidth, Is.EqualTo(expectedWidth));
		}
	}
}