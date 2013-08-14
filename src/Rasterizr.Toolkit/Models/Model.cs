using System;
using System.Collections.Generic;
using Nexus;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Toolkit.Effects;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Toolkit.Models
{
    public class Model
    {
        private readonly List<ModelMesh> _meshes;
        private bool _inputLayoutSet;

        public IEnumerable<ModelMesh> Meshes
        {
            get { return _meshes; }
        }

        public Point3D AxisAlignedBoxMin { get; set; }
        public Point3D AxisAlignedBoxMax { get; set; }
        public Point3D AxisAlignedBoxCentre { get; set; }

        public Model()
        {
            _meshes = new List<ModelMesh>();
            _inputLayoutSet = false;
        }

        public void AddMesh(ModelMesh mesh)
        {
            _meshes.Add(mesh);
        }

        public void SetAxisAlignedBox(Point3D min, Point3D max)
        {
            AxisAlignedBoxMin = min;
            AxisAlignedBoxMax = max;
            AxisAlignedBoxCentre = 0.5f * (min + max);
        }

        public void Draw(DeviceContext context)
        {
            foreach (var mesh in _meshes)
            {
                context.InputAssembler.InputLayout = mesh.InputLayout;
                context.InputAssembler.PrimitiveTopology = mesh.PrimitiveTopology;
                context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(mesh.VertexBuffer, 0, mesh.VertexSize));
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