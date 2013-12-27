using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Rasterizr.Pipeline.Rasterizer;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Pipeline.PixelShader
{
	public class PixelShaderStage : CommonShaderStage<PixelShader>
	{
		private int _outputColorRegister;

	    protected override int BatchSize
		{
			get { return 32; }
		}

	    public PixelShaderStage(Device device)
			: base(device)
		{
		}

	    protected override void OnShaderChanged(PixelShader shader)
		{
			var outputColorRegister = GetSystemValueRegister(Name.Target);
			if (outputColorRegister == null)
				throw new ArgumentException("Shader doesn't contain output color", "shader");
			_outputColorRegister = outputColorRegister.Value;

			base.OnShaderChanged(shader);
		}

		internal IEnumerable<Pixel> Execute(IEnumerable<FragmentQuad> inputs)
		{
			SetShaderConstants();

            foreach (var fragmentQuadBatch in inputs.Batch(BatchSize / 4))
            {
                var fragmentQuads = fragmentQuadBatch.ToList();

                var contextIndex = 0;
                foreach (var fragmentQuad in fragmentQuads)
                {
                    SetShaderInputs(contextIndex + 0, 0, fragmentQuad.Fragment0.Data);
                    SetShaderInputs(contextIndex + 1, 0, fragmentQuad.Fragment1.Data);
                    SetShaderInputs(contextIndex + 2, 0, fragmentQuad.Fragment2.Data);
                    SetShaderInputs(contextIndex + 3, 0, fragmentQuad.Fragment3.Data);
                    contextIndex += 4;
                }

                VirtualMachine.Execute();

                contextIndex = 0;
                for (int i = 0; i < fragmentQuads.Count; i++)
                {
                    var fragmentQuad = fragmentQuads[i];

                    if (fragmentQuad.Fragment0.IsInsideViewport && fragmentQuad.Fragment0.Samples.AnyCovered)
                        yield return GetPixel(ref fragmentQuad.Fragment0, contextIndex + 0);
                    if (fragmentQuad.Fragment1.IsInsideViewport && fragmentQuad.Fragment1.Samples.AnyCovered)
                        yield return GetPixel(ref fragmentQuad.Fragment1, contextIndex + 1);
                    if (fragmentQuad.Fragment2.IsInsideViewport && fragmentQuad.Fragment2.Samples.AnyCovered)
                        yield return GetPixel(ref fragmentQuad.Fragment2, contextIndex + 2);
                    if (fragmentQuad.Fragment3.IsInsideViewport && fragmentQuad.Fragment3.Samples.AnyCovered)
                        yield return GetPixel(ref fragmentQuad.Fragment3, contextIndex + 3);
                    contextIndex += 4;
                }
            }
		}

		private Pixel GetPixel(ref Fragment fragment, int contextIndex)
		{
			var outputs = GetShaderOutputs(contextIndex);
			return new Pixel(fragment.X, fragment.Y)
			{
                Vertices = fragment.Vertices,
				Samples = fragment.Samples,
				Color = outputs[_outputColorRegister],
				PrimitiveID = fragment.PrimitiveID,
                RenderTargetArrayIndex = fragment.RenderTargetArrayIndex
			};
		}
	}
}