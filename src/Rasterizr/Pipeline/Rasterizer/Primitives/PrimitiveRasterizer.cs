using System.Collections.Generic;
using Rasterizr.Math;
using Rasterizr.Pipeline.InputAssembler;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Pipeline.Rasterizer.Primitives
{
	internal abstract class PrimitiveRasterizer
	{
        public ShaderOutputInputBindings OutputInputBindings { get; set; }

        public Box2D ScreenBounds;
        public RasterizerStateDescription RasterizerState { get; set; }

		public bool IsMultiSamplingEnabled { get; set; }
		public int MultiSampleCount { get; set; }

		public FillMode FillMode { get; set; }

		public abstract InputAssemblerPrimitiveOutput Primitive { get; set; }

        public abstract bool ShouldCull(VertexShader.VertexShaderOutput[] vertices);

		public abstract IEnumerable<FragmentQuad> Rasterize();

        protected Point GetSamplePosition(int x, int y, int sampleIndex)
		{
			return MultiSamplingUtility.GetSamplePosition(MultiSampleCount, x, y, sampleIndex);
		}
	}
}