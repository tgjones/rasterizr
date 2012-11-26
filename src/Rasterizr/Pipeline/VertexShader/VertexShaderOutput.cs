using Rasterizr.Math;
using SlimShader;

namespace Rasterizr.Pipeline.VertexShader
{
	internal struct VertexShaderOutput
	{
		public int VertexID;
		public int InstanceID;
		public Vector4 Position;
		public Number4[] Data;
	}
}