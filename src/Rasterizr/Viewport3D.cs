using System.Globalization;
using Nexus;

namespace Rasterizr
{
	public struct Viewport3D
	{
		public int X { get; set; }

		public int Y { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public float MinDepth { get; set; }

		public float MaxDepth { get; set; }

		public Viewport3D(int x, int y, int width, int height, float minDepth, float maxDepth)
			: this()
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
			MinDepth = minDepth;
			MaxDepth = maxDepth;
		}

		public Viewport3D(int x, int y, int width, int height)
			: this()
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
			MinDepth = 0;
			MaxDepth = 1;
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{{X:{0} Y:{1} Width:{2} Height:{3} MinDepth:{4} MaxDepth:{5}}}", new object[] { this.X, this.Y, this.Width, this.Height, this.MinDepth, this.MaxDepth });
		}

		private static bool WithinEpsilon(float a, float b)
		{
			float num = a - b;
			return ((-1.401298E-45f <= num) && (num <= float.Epsilon));
		}

		/// <summary>
		/// Projects a scene point onto the screen.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="projection"></param>
		/// <param name="view"></param>
		/// <param name="world"></param>
		/// <returns></returns>
		public Point3D Project(Point3D source, Matrix3D projection, Matrix3D view, Matrix3D world)
		{
			Matrix3D matrix = world * view * projection;
			Point3D vector = Point3D.Transform(source, matrix);
			float a = (((source.X * matrix.M14) + (source.Y * matrix.M24)) + (source.Z * matrix.M34)) + matrix.M44;
			if (!WithinEpsilon(a, 1f))
				vector = vector / a;
			vector.X = (((vector.X + 1f) * 0.5f) * this.Width) + this.X;
			vector.Y = (((-vector.Y + 1f) * 0.5f) * this.Height) + this.Y;
			vector.Z = (vector.Z * (this.MaxDepth - this.MinDepth)) + this.MinDepth;
			return vector;
		}

		/// <summary>
		/// Projects a screen point into the scene.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="projection"></param>
		/// <param name="view"></param>
		/// <param name="world"></param>
		/// <returns></returns>
		public Point3D Unproject(Point3D source, Matrix3D projection, Matrix3D view, Matrix3D world)
		{
			Point3D position = new Point3D();
			Matrix3D matrix = Matrix3D.Invert(world * view * projection);
			position.X = (((source.X - X) / Width) * 2f) - 1f;
			position.Y = -((((source.Y - Y) / Height) * 2f) - 1f);
			position.Z = (source.Z - this.MinDepth) / (this.MaxDepth - this.MinDepth);
			position = Point3D.Transform(position, matrix);
			float a = (((source.X * matrix.M14) + (source.Y * matrix.M24)) + (source.Z * matrix.M34)) + matrix.M44;
			if (!WithinEpsilon(a, 1f))
				position = position / a;
			return position;
		}

		public float AspectRatio
		{
			get
			{
				if (Height != 0 && Width != 0)
					return Width / (float) Height;
				return 0f;
			}
		}
	}
}