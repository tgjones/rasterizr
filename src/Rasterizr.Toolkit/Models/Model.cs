using System.Collections.Generic;
using Rasterizr.Pipeline.InputAssembler;
using SharpDX;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Toolkit.Models
{
    public class Model
    {
        private readonly List<ModelMesh> _meshes;
        private bool _inputLayoutSet;

        public IList<ModelMesh> Meshes
        {
            get { return _meshes; }
        }

        public Vector3 AxisAlignedBoxMin { get; set; }
        public Vector3 AxisAlignedBoxMax { get; set; }
        public Vector3 AxisAlignedBoxCentre { get; set; }

        public Model()
        {
            _meshes = new List<ModelMesh>();
            _inputLayoutSet = false;
        }

        public void AddMesh(ModelMesh mesh)
        {
            _meshes.Add(mesh);
        }

        public void SetAxisAlignedBox(Vector3 min, Vector3 max)
        {
            AxisAlignedBoxMin = min;
            AxisAlignedBoxMax = max;
            AxisAlignedBoxCentre = 0.5f * (min + max);
        }

        public void Draw(DeviceContext context)
        {
            foreach (var mesh in _meshes)
            {
                if (mesh.PrimitiveTopology != PrimitiveTopology.TriangleList)
                    continue; // TODO

                context.InputAssembler.InputLayout = mesh.InputLayout;
                context.InputAssembler.PrimitiveTopology = mesh.PrimitiveTopology;
                context.InputAssembler.SetVertexBuffers(0,
                    new VertexBufferBinding(mesh.VertexBuffer, 0, mesh.VertexSize));
                context.InputAssembler.SetIndexBuffer(mesh.IndexBuffer, Format.R32_UInt, 0);
                context.PixelShader.SetShaderResources(0, mesh.DiffuseTextureView);

                context.DrawIndexed(mesh.IndexCount, 0, 0);
            }
        }

        public void SetInputLayout(Device device, InputSignatureChunk inputSignature)
        {
            foreach (ModelMesh mesh in _meshes)
                mesh.SetInputLayout(device, inputSignature);
            _inputLayoutSet = true;
        }
    }
}