using SharpDX;

namespace Rasterizr.Toolkit.Models
{
    internal static class ConversionExtensions
    {
        public static Matrix ToMatrix(this Assimp.Matrix4x4 mat)
        {
            var m = new Matrix();
            m.M11 = mat.A1;
            m.M12 = mat.A2;
            m.M13 = mat.A3;
            m.M14 = mat.A4;
            m.M21 = mat.B1;
            m.M22 = mat.B2;
            m.M23 = mat.B3;
            m.M24 = mat.B4;
            m.M31 = mat.C1;
            m.M32 = mat.C2;
            m.M33 = mat.C3;
            m.M34 = mat.C4;
            m.M41 = mat.D1;
            m.M42 = mat.D2;
            m.M43 = mat.D3;
            m.M44 = mat.D4;
            return m;
        }

        public static Vector3 ToVector3(this Assimp.Vector3D vec)
        {
            Vector3 v;
            v.X = vec.X;
            v.Y = vec.Y;
            v.Z = vec.Z;
            return v;
        }

        public static Color ToColor(this Assimp.Color4D color)
        {
            Color c;
            c.R = (byte) (color.R * 255);
            c.G = (byte) (color.G * 255);
            c.B = (byte) (color.B * 255);
            c.A = (byte) (color.A * 255);
            return c;
        }
    }
}