using NUnit.Framework;
using Rasterizr.Pipeline.OutputMerger;
using SlimShader;

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
		    var source = new Number4(1.0f, 0.0f, 0.0f, 0.3f);
		    var destination = new Number4(0.0f, 1.0f, 0.0f, 0.4f);
		    var blendFactor = new Number4();

			// Act.
			var result = blendState.DoBlend(0,
                ref source, ref destination,
                ref blendFactor);

			// Assert.
			Assert.That(result.R, Is.EqualTo(1.0f).Within(0.01f));
			Assert.That(result.G, Is.EqualTo(0.7f).Within(0.01f));
			Assert.That(result.B, Is.EqualTo(0.0f).Within(0.01f));
			Assert.That(result.A, Is.EqualTo(0.3f + (1 - 0.3f) * 0.4f).Within(0.01f));
		}
	}
}