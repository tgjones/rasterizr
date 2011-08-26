using System;
using Rasterizr.PipelineStages.ShaderStages.PixelShader;
using Rasterizr.PipelineStages.TriangleSetup;

namespace Rasterizr.PipelineStages.Rasterizer
{
	/// <summary>
	/// A potential pixel.
	/// </summary>
	public class Fragment
	{
		public int X;
		public int Y;
		public SampleCollection Samples;
		public IPixelShaderInput PixelShaderInput;
		public float Depth { get; set; }

		public Fragment(int x, int y)
		{
			X = x;
			Y = y;
			Samples = new SampleCollection();
		}
	}
}