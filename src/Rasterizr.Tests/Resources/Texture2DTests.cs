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

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void NonPowerOf2TexturesCannotHaveMipMaps()
		{
			// Act.
			new Texture2D(new Device(), new Texture2DDescription
			{
				Width = 31,
				Height = 32,
				ArraySize = 1,
				Format = Format.R8G8B8A8_UInt
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
				MipLevels = 1,
				Format = Format.R8G8B8A8_UInt
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
				ArraySize = 1,
				Format = Format.R8G8B8A8_UInt
			});

			// Act / Assert.
			int actualWidth, actualHeight;
			texture.GetDimensions(mipLevel, out actualWidth, out actualHeight);
			Assert.That(actualWidth, Is.EqualTo(expectedWidth));
			Assert.That(actualHeight, Is.EqualTo(expectedHeight));
		}

		[Test]
		public void CanGenerateMips()
		{
			// Arrange.
			var texture = TextureLoader.CreateTextureFromFile(new Device(), File.OpenRead("Assets/Texture.jpg"));
			var mipLevel1 = TextureLoader.CreateTextureFromFile(new Device(), File.OpenRead("Assets/TextureMip1.jpg"));
			var mipLevel2 = TextureLoader.CreateTextureFromFile(new Device(), File.OpenRead("Assets/TextureMip2.jpg"));

			// Act.
			texture.GenerateMips();

			// Assert.
			Assert.That(texture.GetSubresource(0, 1).Data, Is.EqualTo(mipLevel1.GetSubresource(0, 0).Data));
			Assert.That(texture.GetSubresource(0, 2).Data, Is.EqualTo(mipLevel2.GetSubresource(0, 0).Data));
		}
	}
}