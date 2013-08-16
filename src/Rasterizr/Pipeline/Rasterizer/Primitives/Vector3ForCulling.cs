namespace Rasterizr.Pipeline.Rasterizer.Primitives
{
    internal struct Vector3ForCulling
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3ForCulling(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Converts the vector into a unit vector.
        /// </summary>
        public void Normalize()
        {
            var length = (float) System.Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
            var inv = 1.0f / length;
            X *= inv;
            Y *= inv;
            Z *= inv;
        }

        /// <summary>
        /// Calculates the cross-product, but only of the Z component,
        /// because that's all we need in order to do backface culling.
        /// </summary>
        public static float CrossZ(ref Vector3ForCulling left, ref Vector3ForCulling right)
        {
            return (left.X * right.Y) - (left.Y * right.X);
        }
    }
}