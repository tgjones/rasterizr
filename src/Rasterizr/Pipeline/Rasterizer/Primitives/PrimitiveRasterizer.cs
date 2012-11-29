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

		public bool IsMultiSamplingEnabled { get; set; }
		public int MultiSampleCount { get; set; }

		public FillMode FillMode { get; set; }

		public abstract InputAssemblerPrimitiveOutput Primitive { get; set; }

		protected int[] OutputInputRegisterMappings { get; set; }

		public void Initialize()
		{
			OutputInputRegisterMappings = new int[PixelShaderInputSignture.Parameters.Count];
			for (int i = 0; i < PixelShaderInputSignture.Parameters.Count; i++)
			{
				var parameter = PixelShaderInputSignture.Parameters[i];

				// Grab values from vertex shader outputs.
				OutputInputRegisterMappings[i] = (int) PreviousStageOutputSignature.Parameters.FindRegister(
					parameter.SemanticName, parameter.SemanticIndex);
			}
		}

		public abstract IEnumerable<FragmentQuad> Rasterize();

		protected Vector2 GetSamplePosition(int x, int y, int sampleIndex)
		{
			return MultiSamplingUtility.GetSamplePosition(MultiSampleCount, x, y, sampleIndex);
		}
	}
}