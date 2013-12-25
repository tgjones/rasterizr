// Parts of this file are from the Assimp wrapper here:
// http://interplayoflight.wordpress.com/2013/03/03/sharpdx-and-3d-model-loading/

using System;
using System.IO;
using Assimp;
using Assimp.Configs;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Resources;
using SharpDX;
using Utilities = Rasterizr.Util.Utilities;
using Vector3D = Assimp.Vector3D;

namespace Rasterizr.Toolkit.Models
{
    public class ModelLoader
    {
        private readonly Device _device;
        private readonly TextureLoadHandler _textureLoadHandler;
        private readonly AssimpImporter _importer;
        private string _modelPath;

        public ModelLoader(Device device, TextureLoadHandler textureLoadHandler)
        {
            _device = device;
            _textureLoadHandler = textureLoadHandler;
            _importer = new AssimpImporter();
            _importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));
        }

        public Model Load(string fileName)
        {
            Scene scene = _importer.ImportFile(fileName, PostProcessPreset.TargetRealTimeMaximumQuality);

            //use this directory path to load textures from
            _modelPath = Path.GetDirectoryName(fileName);

            var model = new Model();
            var identity = Matrix.Identity;

            AddVertexData(model, scene, scene.RootNode, _device, ref identity);
            ComputeBoundingBox(model, scene);

            return model;
        }

        /// <summary>
        /// Calculates the bounding box of the whole model.
        /// </summary>
        private void ComputeBoundingBox(Model model, Scene scene)
        {
            var sceneMin = new Vector3(1e10f, 1e10f, 1e10f);
            var sceneMax = new Vector3(-1e10f, -1e10f, -1e10f);
            var transform = Matrix.Identity;

            ComputeBoundingBox(scene, scene.RootNode, ref sceneMin, ref sceneMax, ref transform);

            //set min and max of bounding box
            model.SetAxisAlignedBox(sceneMin, sceneMax);
        }

        /// <summary>
        /// Recursively calculates the bounding box of the whole model.
        /// </summary>
        private void ComputeBoundingBox(Scene scene, Node node,
            ref Vector3 min, ref Vector3 max,
            ref Matrix transform)
        {
            var previousTransform = transform;
            transform = Matrix.Multiply(previousTransform, node.Transform.ToMatrix());

            if (node.HasMeshes)
            {
                foreach (int index in node.MeshIndices)
                {
                    Mesh mesh = scene.Meshes[index];
                    for (int i = 0; i < mesh.VertexCount; i++)
                    {
                        var tmp = mesh.Vertices[i].ToVector3();
                        Vector4 result;
                        Vector3.Transform(ref tmp, ref transform, out result);

                        min.X = System.Math.Min(min.X, result.X);
                        min.Y = System.Math.Min(min.Y, result.Y);
                        min.Z = System.Math.Min(min.Z, result.Z);

                        max.X = System.Math.Max(max.X, result.X);
                        max.Y = System.Math.Max(max.Y, result.Y);
                        max.Z = System.Math.Max(max.Z, result.Z);
                    }
                }
            }

            // Go down the hierarchy if children are present.
            for (int i = 0; i < node.ChildCount; i++)
                ComputeBoundingBox(scene, node.Children[i], ref min, ref max, ref transform);

            transform = previousTransform;
        }

        /// <summary>
        /// Determine the number of elements in the vertex.
        /// </summary>
        private static int GetNumberOfInputElements(Mesh mesh)
        {
            bool hasTexCoords = mesh.HasTextureCoords(0);
            bool hasColors = mesh.HasVertexColors(0);
            bool hasNormals = mesh.HasNormals;
            bool hasTangents = mesh.Tangents != null;
            bool hasBitangents = mesh.BiTangents != null;

            int noofElements = 1;

            if (hasColors)
                noofElements++;

            if (hasNormals)
                noofElements++;

            if (hasTangents)
                noofElements++;

            if (hasBitangents)
                noofElements++;

            if (hasTexCoords)
                noofElements++;

            return noofElements;
        }

        /// <summary>
        /// Create meshes and add vertex and index buffers.
        /// </summary>
        private void AddVertexData(
            Model model, Scene scene, Node node, Device device,
            ref Matrix transform)
        {
            var previousTransform = transform;
            transform = Matrix.Multiply(previousTransform, node.Transform.ToMatrix());

            // Also calculate inverse transpose matrix for normal/tangent/bitagent transformation.
            var invTranspose = transform;
            invTranspose.Invert();
            invTranspose.Transpose();

            if (node.HasMeshes)
            {
                foreach (int index in node.MeshIndices)
                {
                    // Get a mesh from the scene.
                    Mesh mesh = scene.Meshes[index];

                    // Create new mesh to add to model.
                    var modelMesh = new ModelMesh();
                    model.AddMesh(modelMesh);

                    // If mesh has a material extract the diffuse texture, if present.
                    Material material = scene.Materials[mesh.MaterialIndex];
                    if (material != null && material.GetTextureCount(TextureType.Diffuse) > 0)
                    {
                        TextureSlot aiTexture = material.GetTexture(TextureType.Diffuse, 0);
                        using (var fileStream = File.OpenRead(_modelPath + "\\" + Path.GetFileName(aiTexture.FilePath)))
                        {
                            var texture = _textureLoadHandler(_device, fileStream);
                            modelMesh.AddTextureDiffuse(device, texture);
                        }
                    }

                    // Determine the elements in the vertex.
                    bool hasTexCoords = mesh.HasTextureCoords(0);
                    bool hasColors = mesh.HasVertexColors(0);
                    bool hasNormals = mesh.HasNormals;
                    bool hasTangents = mesh.Tangents != null;
                    bool hasBitangents = mesh.BiTangents != null;

                    // Create vertex element list.
                    var vertexElements = new InputElement[GetNumberOfInputElements(mesh)];
                    uint elementIndex = 0;
                    vertexElements[elementIndex++] = new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0);
                    var vertexSize = (short) Utilities.SizeOf<Vector3>();

                    if (hasColors)
                    {
                        vertexElements[elementIndex++] = new InputElement("COLOR", 0, Format.R8G8B8A8_UInt, 0, vertexSize);
                        vertexSize += (short) Utilities.SizeOf<Color>();
                    }
                    if (hasNormals)
                    {
                        vertexElements[elementIndex++] = new InputElement("NORMAL", 0, Format.R32G32B32_Float, 0, vertexSize);
                        vertexSize += (short) Utilities.SizeOf<Vector3>();
                    }
                    if (hasTangents)
                    {
                        vertexElements[elementIndex++] = new InputElement("TANGENT", 0, Format.R32G32B32_Float, 0, vertexSize);
                        vertexSize += (short) Utilities.SizeOf<Vector3>();
                    }
                    if (hasBitangents)
                    {
                        vertexElements[elementIndex++] = new InputElement("BITANGENT", 0, Format.R32G32B32_Float, 0, vertexSize);
                        vertexSize += (short) Utilities.SizeOf<Vector3>();
                    }
                    if (hasTexCoords)
                    {
                        vertexElements[elementIndex++] = new InputElement("TEXCOORD", 0, Format.R32G32_Float, 0, vertexSize);
                        vertexSize += (short) Utilities.SizeOf<Vector2>();
                    }

                    //set the vertex elements and size
                    modelMesh.InputElements = vertexElements;
                    modelMesh.VertexSize = vertexSize;

                    //get pointers to vertex data
                    Vector3D[] positions = mesh.Vertices;
                    Vector3D[] texCoords = mesh.GetTextureCoords(0);
                    Vector3D[] normals = mesh.Normals;
                    Vector3D[] tangents = mesh.Tangents;
                    Vector3D[] biTangents = mesh.BiTangents;
                    Color4D[] colours = mesh.GetVertexColors(0);

                    //also determine primitive type
                    switch (mesh.PrimitiveType)
                    {
                        case PrimitiveType.Point:
                            modelMesh.PrimitiveTopology = PrimitiveTopology.PointList;
                            break;
                        case PrimitiveType.Line:
                            modelMesh.PrimitiveTopology = PrimitiveTopology.LineList;
                            break;
                        case PrimitiveType.Triangle:
                            modelMesh.PrimitiveTopology = PrimitiveTopology.TriangleList;
                            break;
                        default:
                            throw new Exception("ModelLoader::AddVertexData(): Unknown primitive type");
                    }

                    // Create new vertex buffer.
                    var vertexBuffer = device.CreateBuffer(
                        new BufferDescription
                        {
                            BindFlags = BindFlags.VertexBuffer,
                            SizeInBytes = mesh.VertexCount * vertexSize
                        });

                    int byteOffset = 0;
                    for (int i = 0; i < mesh.VertexCount; i++)
                    {
                        {
                            // Add position, after transforming it with accumulated node transform.
                            Vector4 tempResult;
                            Vector3 pos = positions[i].ToVector3();
                            Vector3.Transform(ref pos, ref transform, out tempResult);
                            var result = new Vector3(tempResult.X, tempResult.Y, tempResult.Z);
                            device.ImmediateContext.SetBufferData(vertexBuffer, ref result, byteOffset);
                            byteOffset += Vector3.SizeInBytes;
                        }

                        if (hasColors)
                        {
                            var vertColor = colours[i].ToColor();
                            device.ImmediateContext.SetBufferData(vertexBuffer, ref vertColor, byteOffset);
                            byteOffset += 4;
                        }
                        if (hasNormals)
                        {
                            var normal = normals[i].ToVector3();
                            Vector4 tempResult;
                            Vector3.Transform(ref normal, ref invTranspose, out tempResult);
                            var result = new Vector3(tempResult.X, tempResult.Y, tempResult.Z);
                            device.ImmediateContext.SetBufferData(vertexBuffer, ref result, byteOffset);
                            byteOffset += Vector3.SizeInBytes;
                        }
                        if (hasTangents)
                        {
                            var tangent = tangents[i].ToVector3();
                            Vector4 tempResult;
                            Vector3.Transform(ref tangent, ref invTranspose, out tempResult);
                            var result = new Vector3(tempResult.X, tempResult.Y, tempResult.Z);
                            device.ImmediateContext.SetBufferData(vertexBuffer, ref result, byteOffset);
                            byteOffset += Vector3.SizeInBytes;
                        }
                        if (hasBitangents)
                        {
                            var biTangent = biTangents[i].ToVector3();
                            Vector4 tempResult;
                            Vector3.Transform(ref biTangent, ref invTranspose, out tempResult);
                            var result = new Vector3(tempResult.X, tempResult.Y, tempResult.Z);
                            device.ImmediateContext.SetBufferData(vertexBuffer, ref result, byteOffset);
                            byteOffset += Vector3.SizeInBytes;
                        }
                        if (hasTexCoords)
                        {
                            var result = new Vector2(texCoords[i].X, 1 - texCoords[i].Y);
                            device.ImmediateContext.SetBufferData(vertexBuffer, ref result, byteOffset);
                            byteOffset += Vector2.SizeInBytes;
                        }
                    }

                    // Add it to the mesh.
                    modelMesh.VertexBuffer = vertexBuffer;
                    modelMesh.VertexCount = mesh.VertexCount;
                    modelMesh.PrimitiveCount = mesh.FaceCount;

                    // Get pointer to indices data.
                    var indices = mesh.GetIndices();

                    // Create new index buffer.
                    var indexBuffer = device.CreateBuffer(
                        new BufferDescription
                        {
                            BindFlags = BindFlags.IndexBuffer,
                            SizeInBytes = indices.Length * sizeof(uint)
                        }, indices);

                    // Add it to the mesh.
                    modelMesh.IndexBuffer = indexBuffer;
                    modelMesh.IndexCount = indices.Length;
                }
            }

            // If node has more children process them as well.
            for (var i = 0; i < node.ChildCount; i++)
                AddVertexData(model, scene, node.Children[i], device, ref transform);

            transform = previousTransform;
        }
    }
}