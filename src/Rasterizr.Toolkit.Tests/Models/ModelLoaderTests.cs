using System.Linq;
using NUnit.Framework;
using Rasterizr.Platform.Wpf;
using Rasterizr.Toolkit.Models;

namespace Rasterizr.Toolkit.Tests.Models
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
            const string file = "Assets/Sponza/sponza.3ds";

            // Act.
            var model = modelLoader.Load(file);

            // Assert.
            Assert.That(model, Is.Not.Null);
            Assert.That(model.Meshes, Is.Not.Null);
            Assert.That(model.Meshes, Has.Count.EqualTo(37));

            var expectedIndexCounts = new[]
            {
                576,
                3456,
                10320,
                1440,
                144,
                384,
                2736,
                2544,
                192,
                3456,
                4320,
                2304,
                210,
                2712,
                312,
                1920,
                48,
                1260,
                3300,
                144,
                5100,
                690,
                25056,
                31110,
                6324,
                3060,
                4368,
                12096,
                14931,
                24144,
                16704,
                7515,
                12,
                2439,
                870,
                726,
                2439
            };
            for (int i = 0; i < model.Meshes.Count; i++)
                Assert.That(model.Meshes[i].IndexCount, Is.EqualTo(expectedIndexCounts[i]));

            var expectedVertexCounts = new[]
            {
                200,
                950,
                6761,
                1024,
                64,
                263,
                2224,
                1849,
                144,
                880,
                1530,
                1216,
                76,
                988,
                208,
                1276,
                18,
                436,
                1338,
                56,
                1989,
                274,
                7227,
                9135,
                1760,
                1772,
                2753,
                5477,
                6694,
                7343,
                4805,
                2594,
                6,
                718,
                320,
                340,
                722
            };
            for (int i = 0; i < model.Meshes.Count; i++)
                Assert.That(model.Meshes[i].VertexCount, Is.EqualTo(expectedVertexCounts[i]));

            var expectedFaceCounts = new[]
            {
                192,
                1152,
                3440,
                480,
                48,
                128,
                912,
                848,
                64,
                1152,
                1440,
                768,
                70,
                904,
                104,
                640,
                16,
                420,
                1100,
                48,
                1700,
                230,
                8352,
                10370,
                2108,
                1020,
                1456,
                4032,
                4977,
                8048,
                5568,
                2505,
                4,
                813,
                290,
                242,
                813
            };
            for (int i = 0; i < model.Meshes.Count; i++)
                Assert.That(model.Meshes[i].PrimitiveCount, Is.EqualTo(expectedFaceCounts[i]));

            var expectedTextureWidths = new[]
            {
                1021,
                1021,
                903,
                -1,
                1024,
                1024,
                155,
                423,
                1024,
                207,
                1021,
                903,
                640,
                -1,
                1024,
                423,
                294,
                1021,
                207,
                640,
                1021,
                1021,
                1021,
                -1,
                1021,
                903,
                903,
                423,
                155,
                1021,
                1021,
                640,
                305,
                423,
                305,
                410,
                423
            };
            for (int i = 0; i < model.Meshes.Count; i++)
                if (model.Meshes[i].DiffuseTexture != null)
                    Assert.That(model.Meshes[i].DiffuseTexture.Description.Width, Is.EqualTo(expectedTextureWidths[i]));
        }
    }
}