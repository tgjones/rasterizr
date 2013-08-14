using System.Linq;
using NUnit.Framework;
using Rasterizr.Platform.Wpf;
using Rasterizr.Toolkit.Models;

namespace Rasterizr.Toolkit.Tests
{
    [TestFixture]
    public class ModelLoaderTests
    {
        [Test]
        public void CanImport3dsModel()
        {
            // Arrange.
            var device = new Device();
            var modelLoader = new ModelLoader(device, TextureLoader.CreateTextureFromStream);
            const string file = "Models/85-nissan-fairlady.3ds";

            // Act.
            var model = modelLoader.Load(file);

            // Assert.
            Assert.That(model, Is.Not.Null);
            Assert.That(model.Meshes, Is.Not.Null);
            Assert.That(model.Meshes.Count(), Is.EqualTo(202));
        }
    }
}