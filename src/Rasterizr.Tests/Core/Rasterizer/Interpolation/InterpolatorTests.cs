using NUnit.Framework;
using Nexus;
using Rasterizr.Core.Rasterizer.Interpolation;

namespace Rasterizr.Tests.Core.Rasterizer.Interpolation
{
	[TestFixture]
	public class InterpolatorTests
	{
		[Test]
		public void CanLinearInterpolateColorFValue()
		{
			// Arrange.
			var color1 = ColorsF.Red;
			var color2 = ColorsF.Green;
			var color3 = ColorsF.Blue;

			// Act.
			var result = Interpolator.Linear(0.3f, 0.3f, 0.4f, color1, color2, color3);

			// Assert.
			Assert.That(result, Is.InstanceOf<ColorF>());
			Assert.That(((ColorF) result).R, Is.EqualTo(0.3f).Within(0.01f));
			Assert.That(((ColorF) result).G, Is.EqualTo(0.3f).Within(0.01f));
			Assert.That(((ColorF) result).B, Is.EqualTo(0.4f).Within(0.01f));
		}
	}
}