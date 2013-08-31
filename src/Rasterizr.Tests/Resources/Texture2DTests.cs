using System;
using System.IO;
using NUnit.Framework;
using Rasterizr.Platform.Wpf;
using Rasterizr.Resources;

namespace Rasterizr.Tests.Resources
{
	[TestFixture]
	public class Texture2DTests
	{
		[TestCase(32, 32, 6)]
        [TestCase(31, 32, 6)]
        [TestCase(7, 16, 5)]
		public void TextureHasCorrectNumberOfMipMapLevels(int width, int height, int expectedNumberOfLevels)
		{
			// Arrange.
			var texture = new Texture2D(new Device(), new Texture2DDescription
			{
				Width = width,
				Height = height,
				ArraySize = 1
			});
			int actualWidth, actualHeight, numberOfLevels;

			// Act.
            texture.GetDimensions(0, out actualWidth, out actualHeight, out numberOfLevels);

			// Assert.
            Assert.That(actualWidth, Is.EqualTo(width));
            Assert.That(actualHeight, Is.EqualTo(height));
            Assert.That(numberOfLevels, Is.EqualTo(expectedNumberOfLevels));
		}

		[TestCase(0, 64, 32)]
		[TestCase(1, 32, 16)]
		[TestCase(2, 16, 8)]
		[TestCase(3, 8, 4)]
		[TestCase(4, 4, 2)]
		[TestCase(5, 2, 1)]
		[TestCase(6, 1, 1)]
		public void MipMapsHaveCorrectDimensionsForPowerOfTwoTexture(int mipLevel, int expectedWidth, int expectedHeight)
		{
			// Arrange.
			var texture = new Texture2D(new Device(), new Texture2DDescription
			{
				Width = 64,
				Height = 32,
				ArraySize = 1
			});

			// Act / Assert.
			int actualWidth, actualHeight;
			texture.GetDimensions(mipLevel, out actualWidth, out actualHeight);
			Assert.That(actualWidth, Is.EqualTo(expectedWidth));
			Assert.That(actualHeight, Is.EqualTo(expectedHeight));
		}

        [TestCase(0, 5, 16)]
        [TestCase(1, 2, 8)]
        [TestCase(2, 1, 4)]
        [TestCase(3, 1, 2)]
        [TestCase(4, 1, 1)]
        public void MipMapsHaveCorrectDimensionsForNonPowerOfTwoTexture(int mipLevel, int expectedWidth, int expectedHeight)
        {
            // Arrange.
            var texture = new Texture2D(new Device(), new Texture2DDescription
            {
                Width = 5,
                Height = 16,
                ArraySize = 1
            });

            // Act / Assert.
            int actualWidth, actualHeight;
            texture.GetDimensions(mipLevel, out actualWidth, out actualHeight);
            Assert.That(actualWidth, Is.EqualTo(expectedWidth));
            Assert.That(actualHeight, Is.EqualTo(expectedHeight));
        }

		[Test, Ignore("Doesn't yet work")]
		public void CanGenerateMips()
		{
			// Arrange.
			var texture = TextureLoader.CreateTextureFromStream(new Device(), File.OpenRead("Assets/Texture.png"));
			var mipLevel1 = TextureLoader.CreateTextureFromStream(new Device(), File.OpenRead("Assets/TextureMip1.png"));
			var mipLevel2 = TextureLoader.CreateTextureFromStream(new Device(), File.OpenRead("Assets/TextureMip2.png"));

			// Act.
			texture.GenerateMips();

			// Assert.
			var textureMipLevel1Data = texture.GetSubresource(0, 1).Data;
			var textureMipLevel2Data = texture.GetSubresource(0, 2).Data;
			var mipLevel1Data = mipLevel1.GetSubresource(0, 0).Data;
			var mipLevel2Data = mipLevel2.GetSubresource(0, 0).Data;
			Assert.That(textureMipLevel1Data, Is.EqualTo(mipLevel1Data));
			Assert.That(textureMipLevel2Data, Is.EqualTo(mipLevel2Data));
		}
	}
}