using SlimShader.Chunks.Common;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Pipeline.Rasterizer
{
    internal class ShaderOutputInputBinding
    {
        public int Register;
        public ComponentMask ComponentMask;
        public InterpolationMode InterpolationMode;
        public Name SystemValueType;
    }
}