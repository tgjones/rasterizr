using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Rasterizr.Diagnostics;
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
			Device.Loggers.BeginOperation(OperationType.PixelShaderStageSetShader, shader);

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
                foreach (var fragmentQuad in fragmentQuads)
                {
                    if (fragmentQuad.Fragment0.Samples.AnyCovered)
                        yield return GetPixel(fragmentQuad.Fragment0.X, fragmentQuad.Fragment0.Y, fragmentQuad.Fragment0.Samples, contextIndex + 0, fragmentQuad.Fragment0.PrimitiveID);
                    if (fragmentQuad.Fragment1.Samples.AnyCovered)
                        yield return GetPixel(fragmentQuad.Fragment1.X, fragmentQuad.Fragment1.Y, fragmentQuad.Fragment1.Samples, contextIndex + 1, fragmentQuad.Fragment1.PrimitiveID);
                    if (fragmentQuad.Fragment2.Samples.AnyCovered)
                        yield return GetPixel(fragmentQuad.Fragment2.X, fragmentQuad.Fragment2.Y, fragmentQuad.Fragment2.Samples, contextIndex + 2, fragmentQuad.Fragment2.PrimitiveID);
                    if (fragmentQuad.Fragment3.Samples.AnyCovered)
                        yield return GetPixel(fragmentQuad.Fragment3.X, fragmentQuad.Fragment3.Y, fragmentQuad.Fragment3.Samples, contextIndex + 3, fragmentQuad.Fragment3.PrimitiveID);
                    contextIndex += 4;
                }
            }
		}

		private Pixel GetPixel(int x, int y, Samples samples, int contextIndex, int primitiveID)
		{
			var outputs = GetShaderOutputs(contextIndex);
			return new Pixel(x, y)
			{
				Samples = samples,
				Color = outputs[_outputColorRegister],
				PrimitiveID = primitiveID
			};
		}
	}
}