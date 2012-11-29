using System.Collections.Generic;
using Rasterizr.Math;
using Rasterizr.Pipeline.InputAssembler;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Pipeline.Rasterizer.Primitives
{
	internal abstract class PrimitiveRasterizer
	{
		public OutputSignatureChunk PreviousStageOutputSignature { get; set; }
		public InputSignatureChunk PixelShaderInputSignture { get; set; }
		public int MultiSampleCount { get; set; }
		public FillMode FillMode { get; set; }

		public abstract InputAssemblerPrimitiveOutput Primitive { get; set; }

		public abstract IEnumerable<FragmentQuad> Rasterize();

		protected Vector2 GetSamplePosition(int x, int y, int sampleIndex)
		{
			return MultiSamplingUtility.GetSamplePosition(MultiSampleCount, x, y, sampleIndex);
		}
	}
}