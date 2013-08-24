using SlimShader;

namespace Rasterizr.Toolkit
{
    public static class Number4Extensions
    {
        public static Color ToColor(this Number4 value)
        {
            return new Color(
                (byte) (value.R * 255.0f),
                (byte) (value.G * 255.0f),
                (byte) (value.B * 255.0f),
                (byte) (value.A * 255.0f));
        }
    }
}