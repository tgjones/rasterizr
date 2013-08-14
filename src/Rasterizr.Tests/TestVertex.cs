using Rasterizr.Pipeline.InputAssembler;
using SharpDX;

namespace Rasterizr.Tests
{
	internal static class TestVertex
	{
		public struct PositionNormalTexture
		{
			public static int SizeInBytes
			{
                get { return Vector3.SizeInBytes + Vector3.SizeInBytes + Vector2.SizeInBytes; }
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

            public Vector3 Position;
            public Vector3 Normal;
            public Vector2 TexCoord;

            public PositionNormalTexture(Vector3 position, Vector3 normal, Vector2 texCoord)
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
                get { return Vector3.SizeInBytes + Vector3.SizeInBytes; }
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

            public Vector3 Position;
            public Vector3 Normal;

            public PositionNormal(Vector3 position, Vector3 normal)
			{
				Position = position;
				Normal = normal;
			}
		} 
	}
}