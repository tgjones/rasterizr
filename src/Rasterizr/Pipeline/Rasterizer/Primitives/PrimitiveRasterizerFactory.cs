using System;
using Rasterizr.Pipeline.InputAssembler;

namespace Rasterizr.Pipeline.Rasterizer.Primitives
{
    internal static class PrimitiveRasterizerFactory
    {
        public static PrimitiveRasterizer CreateRasterizer(
            PrimitiveTopology primitiveTopology,
            RasterizerStateDescription rasterizerState, 
            int multiSampleCount, 
            ShaderOutputInputBindings outputInputBindings, 
            ref Viewport viewport,
            Func<int, int, bool> fragmentQuadFilter)
        {
            switch (primitiveTopology)
            {
                case PrimitiveTopology.PointList:
                    throw new NotImplementedException();
                case PrimitiveTopology.LineList:
                case PrimitiveTopology.LineStrip:
                    throw new NotImplementedException();
                case PrimitiveTopology.TriangleList:
                case PrimitiveTopology.TriangleStrip:
                    return new TriangleRasterizer(
                        rasterizerState, multiSampleCount,
                        outputInputBindings, ref viewport,
                        fragmentQuadFilter);
                default:
                    throw new ArgumentOutOfRangeException("primitiveTopology");
            }
        }
    }
}