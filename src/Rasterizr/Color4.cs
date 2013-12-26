using System.Runtime.InteropServices;

namespace Rasterizr
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Color4
    {
        /// <summary>
        /// The Black color (0, 0, 0, 1).
        /// </summary>
        public static readonly Color4 Black = new Color4(0.0f, 0.0f, 0.0f, 1.0f);

        /// <summary>
        /// The White color (1, 1, 1, 1).
        /// </summary>
        public static readonly Color4 White = new Color4(1.0f, 1.0f, 1.0f, 1.0f);

        /// <summary>
        /// The CornflowerBlue color (0.39, 0.58, 0.93, 1).
        /// </summary>
        public static readonly Color4 CornflowerBlue = new Color4(0.39f, 0.58f, 0.93f, 1.0f);

        /// <summary>
        /// The Blue color (0, 0, 1, 1).
        /// </summary>
        public static readonly Color4 Blue = new Color4(0.0f, 0.0f, 1.0f, 1.0f);

        /// <summary>
        /// The red component of the color.
        /// </summary>
        public float R;

        /// <summary>
        /// The green component of the color.
        /// </summary>
        public float G;

        /// <summary>
        /// The blue component of the color.
        /// </summary>
        public float B;

        /// <summary>
        /// The alpha component of the color.
        /// </summary>
        public float A;

        /// <summary>
        /// Initializes a new instance of the <see cref="Color4"/> struct.
        /// </summary>
        /// <param name="red">The red component of the color.</param>
        /// <param name="green">The green component of the color.</param>
        /// <param name="blue">The blue component of the color.</param>
        /// <param name="alpha">The alpha component of the color.</param>
        public Color4(float red, float green, float blue, float alpha)
        {
            R = red;
            G = green;
            B = blue;
            A = alpha;
        }
    }
}