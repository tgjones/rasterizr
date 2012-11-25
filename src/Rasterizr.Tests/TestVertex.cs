using Nexus;
using Rasterizr.Pipeline.InputAssembler;

namespace Rasterizr.Tests
{
	internal static class TestVertex
	{
		public struct PositionNormalTexture
		{
			public static int SizeInBytes
			{
				get { return Point3D.SizeInBytes + Vector3D.SizeInBytes + Point2D.SizeInBytes; }
			}

			public static InputElement[] InputElements
			{
				get
				{
					return new[]
					{
						new InputElement("SV_Position", 0, Format.R32G32B32_Float, 0),
						new InputElement("NORMAL", 0, Format.R32G32B32_Float, 0),
						new InputElement("TEXCOORD", 0, Format.R32G32_Float, 0)
					};
				}
			}

			public Point3D Position;
			public Vector3D Normal;
			public Point2D TexCoord;

			public PositionNormalTexture(Point3D position, Vector3D normal, Point2D texCoord)
			{
				Position = position;
				Normal = normal;
				TexCoord = texCoord;
			}
		}

		public struct PositionNormal
		{
			public static int SizeInBytes
			{
				get { return Point3D.SizeInBytes + Vector3D.SizeInBytes; }
			}

			public static InputElement[] InputElements
			{
				get
				{
					return new[]
					{
						new InputElement("SV_Position", 0, Format.R32G32B32_Float, 0),
						new InputElement("NORMAL", 0, Format.R32G32B32_Float, 0)
					};
				}
			}

			public Point3D Position;
			public Vector3D Normal;

			public PositionNormal(Point3D position, Vector3D normal)
			{
				Position = position;
				Normal = normal;
			}
		} 
	}
}