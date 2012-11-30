using NUnit.Framework;
using Rasterizr.Math;
using Rasterizr.Pipeline.OutputMerger;

namespace Rasterizr.Tests.Pipeline.OutputMerger
{
	[TestFixture]
	public class BlendStateTests
	{
		[Test]
		public void AlphaBlendStateIsValid()
		{
			// Arrange.
			var device = new Device();
			var blendState = new BlendState(device, BlendStateDescription.AlphaBlend);

			// Act.
			var result = blendState.DoBlend(0,
				new Color4F(1.0f, 0.0f, 0.0f, 0.3f),
				new Color4F(0.0f, 1.0f, 0.0f, 0.4f),
				new Color4F());

			// Assert.
			Assert.That(result.R, Is.EqualTo(1.0f).Within(0.01f));
			Assert.That(result.G, Is.EqualTo(0.7f).Within(0.01f));
			Assert.That(result.B, Is.EqualTo(0.0f).Within(0.01f));
			Assert.That(result.A, Is.EqualTo(0.3f + (1 - 0.3f) * 0.4f).Within(0.01f));
		}
	}
}