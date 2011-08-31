using Rasterizr.ShaderStages.PixelShader;

namespace Rasterizr.Rasterizer
{
	/// <summary>
	/// A potential pixel.
	/// </summary>
	public class Fragment
	{
		public int X;
		public int Y;
		public SampleCollection Samples;
		public object PixelShaderInput;
		public float Depth { get; set; }

		public Fragment(int x, int y)
		{
			X = x;
			Y = y;
			Samples = new SampleCollection();
		}
	}
}