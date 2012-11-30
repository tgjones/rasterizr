using NUnit.Framework;
using Rasterizr.Pipeline.OutputMerger;

namespace Rasterizr.Tests.Pipeline.OutputMerger
{
	[TestFixture]
	public class DepthStencilStateTests
	{
		[Test]
		public void DefaultDepthStencilStateIsValid()
		{
			// Arrange.
			var device = new Device();
			var depthStencilState = new DepthStencilState(device, DepthStencilStateDescription.Default);

			// Act / Assert.
			Assert.That(depthStencilState.DepthTestPasses(0.3f, 0.5f), Is.True);
			Assert.That(depthStencilState.DepthTestPasses(0.5f, 0.5f), Is.False);
			Assert.That(depthStencilState.DepthTestPasses(0.7f, 0.5f), Is.False);
		}
	}
}