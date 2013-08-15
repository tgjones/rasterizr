using Rasterizr.Pipeline;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Resources;
using SlimShader.Chunks.Xsgn;
using Buffer = Rasterizr.Resources.Buffer;

namespace Rasterizr.Toolkit.Models
{
    public class ModelMesh
    {
        public InputElement[] InputElements { get; set; }
        public InputLayout InputLayout { get; set; }
        public int VertexSize { get; set; }
        public Buffer VertexBuffer { get; set; }
        public Buffer IndexBuffer { get; set; }
        public int VertexCount { get; set; }
        public int IndexCount { get; set; }
        public int PrimitiveCount { get; set; }
        public PrimitiveTopology PrimitiveTopology { get; set; }
        public Texture2D DiffuseTexture { get; set; }
        public ShaderResourceView DiffuseTextureView { get; set; }

        public void AddTextureDiffuse(Device device, Texture2D texture)
        {
            DiffuseTexture = texture;
            DiffuseTextureView = device.CreateShaderResourceView(DiffuseTexture);
        }

        public void SetInputLayout(Device device, InputSignatureChunk inputSignature)
        {
            InputLayout = device.CreateInputLayout(InputElements, inputSignature);
        }
    }
}