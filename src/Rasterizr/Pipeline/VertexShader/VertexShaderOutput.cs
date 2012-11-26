using Rasterizr.Math;

namespace Rasterizr.Pipeline.VertexShader
{
	internal struct VertexShaderOutput
	{
		public int VertexID;
		public int InstanceID;
		public Vector4 Position;
		public Vector4[] Data;
	}
}