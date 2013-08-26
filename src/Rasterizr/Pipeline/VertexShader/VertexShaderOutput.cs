using SlimShader;

namespace Rasterizr.Pipeline.VertexShader
{
	internal struct VertexShaderOutput
	{
		public bool IsStripCut;
		public int VertexID;
		public int InstanceID;
		public Number4 Position;
	    public Number4[] InputData;
		public Number4[] OutputData;
	}
}