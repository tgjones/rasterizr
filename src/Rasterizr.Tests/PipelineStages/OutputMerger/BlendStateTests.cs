using NUnit.Framework;
using Nexus;
using Rasterizr.PipelineStages.OutputMerger;

namespace Rasterizr.Tests.PipelineStages.OutputMerger
{
	[TestFixture]
	public class BlendStateTests
	{
		[Test]
		public void AlphaBlendStateIsValid()
		{
			// Arrange.
			var blendState = BlendState.AlphaBlend;

			// Act.
			var result = blendState.DoBlend(new ColorF(0.3f, 1.0f, 0.0f, 0.0f), new ColorF(0.4f, 0.0f, 1.0f, 0.0f));

			// Assert.
			Assert.That(result.R, Is.EqualTo(1.0f).Within(0.01f));
			Assert.That(result.G, Is.EqualTo(0.7f).Within(0.01f));
			Assert.That(result.B, Is.EqualTo(0.0f).Within(0.01f));
			Assert.That(result.A, Is.EqualTo(0.3f + (1 - 0.3f) * 0.4f).Within(0.01f));
		}
	}
}