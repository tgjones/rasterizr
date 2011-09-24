using NUnit.Framework;
using Rasterizr.Core.OutputMerger;

namespace Rasterizr.Tests.Core.OutputMerger
{
	[TestFixture]
	public class DepthStencilStateTests
	{
		[Test]
		public void DefaultDepthStencilStateIsValid()
		{
			// Arrange.
			var depthStencilState = DepthStencilState.Default;

			// Act / Assert.
			Assert.That(depthStencilState.DepthTestPasses(0.3f, 0.5f), Is.True);
			Assert.That(depthStencilState.DepthTestPasses(0.5f, 0.5f), Is.True);
			Assert.That(depthStencilState.DepthTestPasses(0.7f, 0.5f), Is.False);
		}
	}
}