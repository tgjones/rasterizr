using NUnit.Framework;
using SlimShader.ObjectModel;
using SlimShader.Parser;

namespace SlimShader.Tests.Parser
{
	[TestFixture]
	public class DecoderExtensionsTests
	{
		[Test]
		public void CanDecodeSamplerMode()
		{
			// Arrange.
			const uint codedValue = 67115096;

			// Act.
			var decodedValue = (ResourceDimension) codedValue.DecodeValue(11, 15);

			// Assert.
			Assert.That(decodedValue, Is.EqualTo(ResourceDimension.Texture2D));
		}
	}
}