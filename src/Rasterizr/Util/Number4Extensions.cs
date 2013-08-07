using Rasterizr.Math;
using SlimShader;

namespace Rasterizr.Util
{
    public static class Number4Extensions
    {
        public static Color4 ToColor4(this Number4 value)
        {
            return new Color4(
                (byte) (value.R * 255.0f),
                (byte) (value.G * 255.0f),
                (byte) (value.B * 255.0f),
                (byte) (value.A * 255.0f));
        }
    }
}