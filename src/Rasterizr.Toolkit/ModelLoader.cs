using System.Collections.Generic;
using System.IO;
using Assimp;
using Assimp.Unmanaged;

namespace Rasterizr.Toolkit
{
	public static class ModelLoader
	{
        public static Model FromFile(Device device, string file)
        {
            // Preload AssimpLibrary if not already loaded
            if (!AssimpLibrary.Instance.LibraryLoaded)
            {
                var rootPath = Path.GetDirectoryName(typeof(AssimpLibrary).Assembly.Location);
                AssimpLibrary.Instance.LoadLibrary(Path.Combine(rootPath, AssimpLibrary.Instance.DefaultLibraryPath32Bit), Path.Combine(rootPath, AssimpLibrary.Instance.DefaultLibraryPath64Bit));
            }

            var importer = new AssimpImporter();
            var steps = PostProcessSteps.FlipUVs | PostProcessSteps.FlipWindingOrder
                | PostProcessPreset.TargetRealTimeMaximumQuality;

            var scene = importer.ImportFile(file, steps);
            return ProcessScene(scene);

            //// Sort scene meshes by transparency.
            //var sortedSceneMeshes = scene.Meshes;
            //sortedSceneMeshes.Sort((l, r) => r.Material.Transparency.CompareTo(l.Material.Transparency));

            //List<ModelMesh> modelMeshes = new List<ModelMesh>();
            //foreach (Mesh mesh in sortedSceneMeshes)
            //{
            //    ModelMesh modelMesh = new ModelMesh(device);
            //    modelMesh.Name = mesh.Name;
            //    modelMeshes.Add(modelMesh);

            //    modelMesh.Indices = mesh.Indices;

            //    modelMesh.Vertices = new List<VertexPositionNormalTexture>();
            //    for (int i = 0; i < mesh.Positions.Count; ++i)
            //    {
            //        Point2D texCoord = (mesh.TextureCoordinates.Count > i)
            //                            ? mesh.TextureCoordinates[i].Xy
            //                            : Point2D.Zero;
            //        modelMesh.Vertices.Add(new VertexPositionNormalTexture(
            //            mesh.Positions[i], mesh.Normals[i], texCoord));
            //    }

            //    var effect = new BasicEffect(device);

            //    //if (!string.IsNullOrEmpty(mesh.Material.DiffuseTextureName))
            //    //{
            //    //	effect.Texture = Texture2D.FromFile(mesh.Material.DiffuseTextureName);
            //    //	effect.TextureEnabled = true;
            //    //}

            //    effect.DiffuseColor = mesh.Material.DiffuseColor;
            //    effect.SpecularColor = mesh.Material.SpecularColor;
            //    effect.SpecularPower = mesh.Material.Shininess;
            //    effect.Alpha = mesh.Material.Transparency;

            //    modelMesh.Effect = effect;
            //}

            //Model model = new Model(modelMeshes);
            //return model;
        }

        private static Model ProcessScene(Scene aiScene)
        {
            var result = new Model(null);

            var meshNodes = new Dictionary<Node, int>();
            CollectMeshNodes(aiScene.RootNode, meshNodes);

            return result;
        }

        private static void CollectMeshNodes(Node node, Dictionary<Node, int> meshNodes)
        {
            if (node.HasMeshes)
                RegisterNode(node, meshNodes);

            if (node.HasChildren)
                foreach (var child in node.Children)
                    CollectMeshNodes(child, meshNodes);
        }

        private static void RegisterNode(Node node, Dictionary<Node, int> nodeMap)
        {
            while (node != null)
            {
                if (!nodeMap.ContainsKey(node))
                    nodeMap.Add(node, 0);
                else
                    break;

                node = node.Parent;
            }
        }
	}
}