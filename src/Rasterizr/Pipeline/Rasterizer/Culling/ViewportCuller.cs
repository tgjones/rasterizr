using SlimShader;

namespace Rasterizr.Pipeline.Rasterizer.Culling
{
    internal static class ViewportCuller
    {
        public static bool ShouldCullTriangle(VertexShader.VertexShaderOutput[] vertices)
        {
            var v0ClipCode = CalculateClipCode(ref vertices[0].Position);
            var v1ClipCode = CalculateClipCode(ref vertices[1].Position);
            var v2ClipCode = CalculateClipCode(ref vertices[2].Position);

            var result = v0ClipCode & v1ClipCode & v2ClipCode;

            return (result != 0);
        }

        public static bool ShouldCullLine(VertexShader.VertexShaderOutput[] vertices)
        {
            var v0ClipCode = CalculateClipCode(ref vertices[0].Position);
            var v1ClipCode = CalculateClipCode(ref vertices[1].Position);

            var result = v0ClipCode & v1ClipCode;

            return (result != 0);
        }

        public static bool ShouldCullPoint(VertexShader.VertexShaderOutput[] vertices)
        {
            var v0ClipCode = CalculateClipCode(ref vertices[0].Position);

            return v0ClipCode != 0;
        }

        private static uint CalculateClipCode(ref Number4 position)
        {
            var w = position.W;

            uint result = 0;

            if (position.X < -w)
                result |= 1;
            if (position.X > w)
                result |= 2;

            if (position.Y < -w)
                result |= 4;
            if (position.Y > w)
                result |= 8;

            if (position.Z < 0)
                result |= 16;
            if (position.Z > w)
                result |= 32;

            // TODO: 0 < w ?

            return result;
        }
    }
}