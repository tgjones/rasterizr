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
		public OutputSignatureChunk PreviousStageOutputSignature { get; set; }
		public InputSignatureChunk PixelShaderInputSignature { get; set; }
        public IEnumerable<PixelShaderInputRegisterDeclarationToken> InputRegisterDeclarations { get; set; }

	    public Box2D ScreenBounds;
        public RasterizerStateDescription RasterizerState { get; set; }

		public bool IsMultiSamplingEnabled { get; set; }
		public int MultiSampleCount { get; set; }

		public FillMode FillMode { get; set; }

		public abstract InputAssemblerPrimitiveOutput Primitive { get; set; }

		protected int[] OutputInputRegisterMappings { get; set; }
        protected InterpolationMode[] InputRegisterInterpolationModes { get; set; }

		public void Initialize()
		{
			OutputInputRegisterMappings = new int[PixelShaderInputSignature.Parameters.Count];
			for (int i = 0; i < PixelShaderInputSignature.Parameters.Count; i++)
			{
				var parameter = PixelShaderInputSignature.Parameters[i];

				// Grab values from vertex shader outputs.
				OutputInputRegisterMappings[i] = (int) PreviousStageOutputSignature.Parameters.FindRegister(
					parameter.SemanticName, parameter.SemanticIndex);
			}

            InputRegisterInterpolationModes = new InterpolationMode[PixelShaderInputSignature.Parameters.Count];
		    foreach (var declaration in InputRegisterDeclarations)
		        InputRegisterInterpolationModes[declaration.Operand.Indices[0].Value] = declaration.InterpolationMode;
		}

        public abstract bool ShouldCull(VertexShader.VertexShaderOutput[] vertices);

		public abstract IEnumerable<FragmentQuad> Rasterize();

        protected Point GetSamplePosition(int x, int y, int sampleIndex)
		{
			return MultiSamplingUtility.GetSamplePosition(MultiSampleCount, x, y, sampleIndex);
		}
	}
}