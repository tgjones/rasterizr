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
		[Test]
		public void TextureHasCorrectNumberOfMipMapLevels()
		{
			// Arrange.
			var texture = new Texture2D(new Device(), new Texture2DDescription
			{
				Width = 32,
				Height = 32,
				ArraySize = 1
			});
			int width, height, numberOfLevels;

			// Act.
			texture.GetDimensions(0, out width, out height, out numberOfLevels);

			// Assert.
			Assert.That(width, Is.EqualTo(32));
			Assert.That(height, Is.EqualTo(32));
			Assert.That(numberOfLevels, Is.EqualTo(6));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void NonPowerOf2TexturesCannotHaveMipMaps()
		{
			// Act.
			new Texture2D(new Device(), new Texture2DDescription
			{
				Width = 31,
				Height = 32,
				ArraySize = 1
			});
		}

		[Test]
		public void NonPowerOf2TexturesWithoutMipMapsAreAllowed()
		{
			// Act / Assert.
			Assert.DoesNotThrow(() => new Texture2D(new Device(), new Texture2DDescription
			{
				Width = 31,
				Height = 32,
				ArraySize = 1,
				MipLevels = 1
			}));
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