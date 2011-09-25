using NUnit.Framework;
using Rasterizr.Core.ShaderCore;

namespace Rasterizr.Tests.Core.ShaderCore
{
	[TestFixture]
	public class Texture2DTests
	{
		[Test]
		public void TextureHasCorrectNumberOfMipMapLevels()
		{
			// Arrange.
			var texture = new Texture2D(32, 32, true);
			int width, height, numberOfLevels;

			// Act.
			texture.GetDimensions(0, out width, out height, out numberOfLevels);

			// Assert.
			Assert.That(width, Is.EqualTo(32));
			Assert.That(height, Is.EqualTo(32));
			Assert.That(numberOfLevels, Is.EqualTo(6));
		}

		[Test]
		public void MipMapsHaveCorrectDimensions()
		{
			// Arrange.
			var texture = new Texture2D(32, 32, true);

			// Act / Assert.
			AssertMipMapHasCorrectDimensions(texture, 0, 32, 32);
			AssertMipMapHasCorrectDimensions(texture, 1, 16, 16);
			AssertMipMapHasCorrectDimensions(texture, 2, 8, 8);
			AssertMipMapHasCorrectDimensions(texture, 3, 4, 4);
			AssertMipMapHasCorrectDimensions(texture, 4, 2, 2);
			AssertMipMapHasCorrectDimensions(texture, 5, 1, 1);
		}

		private static void AssertMipMapHasCorrectDimensions(Texture2D texture, int mipLevel, int expectedWidth, int expectedHeight)
		{
			int width, height;
			texture.GetDimensions(mipLevel, out width, out height);
			Assert.That(width, Is.EqualTo(expectedWidth));
			Assert.That(height, Is.EqualTo(expectedHeight));
		}
	}
}