using NUnit.Framework;
using Rasterizr.Toolkit.Effects;

namespace Rasterizr.Toolkit.Tests.Effects
{
    [TestFixture]
    public class BasicEffectCodeTests
    {
        [Test]
        public void ShadersAreLoadedFromEmbeddedResources()
        {
            Assert.That(BasicEffectCode.VertexShaderCode, Is.Not.Null);
            Assert.That(BasicEffectCode.VertexShaderCode, Has.Length.GreaterThan(0));

            Assert.That(BasicEffectCode.PixelShaderCode, Is.Not.Null);
            Assert.That(BasicEffectCode.PixelShaderCode, Has.Length.GreaterThan(0));
        }
    }
}