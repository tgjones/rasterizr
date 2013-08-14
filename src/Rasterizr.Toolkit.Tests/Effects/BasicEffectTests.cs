using NUnit.Framework;
using Rasterizr.Toolkit.Effects;

namespace Rasterizr.Toolkit.Tests.Effects
{
    [TestFixture]
    public class BasicEffectTests
    {
        [Test]
        public void CanCreateBasicEffect()
        {
            // Arrange.
            var deviceContext = new Device().ImmediateContext;

            // Act.
            var effect = new BasicEffect(deviceContext);

            // Assert.
            Assert.That(effect, Is.Not.Null);
            Assert.That(effect.DeviceContext, Is.EqualTo(deviceContext));
            Assert.That(effect.VertexShader, Is.Not.Null);
            Assert.That(effect.PixelShader, Is.Not.Null);
        }
    }
}