using System;
using System.Collections.Generic;
using Rasterizr.Diagnostics;
using Rasterizr.Pipeline.Rasterizer;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Pipeline.PixelShader
{
	public class PixelShaderStage : CommonShaderStage<PixelShader>
	{
		private int _outputColorRegister;

		protected override int NumShaderExecutionContexts
		{
			get { return 4; }
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

			// Process groups of four fragments together.
			foreach (var input in inputs)
			{
				SetShaderInputs(0, 0, input.Fragment0.Data);
				SetShaderInputs(1, 0, input.Fragment1.Data);
				SetShaderInputs(2, 0, input.Fragment2.Data);
				SetShaderInputs(3, 0, input.Fragment3.Data);

				VirtualMachine.Execute();

				if (input.Fragment0.Samples.AnyCovered)
					yield return GetPixel(input.Fragment0.X, input.Fragment0.Y, input.Fragment0.Samples, 0, input.Fragment0.PrimitiveID);
				if (input.Fragment1.Samples.AnyCovered)
					yield return GetPixel(input.Fragment1.X, input.Fragment1.Y, input.Fragment1.Samples, 1, input.Fragment1.PrimitiveID);
				if (input.Fragment2.Samples.AnyCovered)
					yield return GetPixel(input.Fragment2.X, input.Fragment2.Y, input.Fragment2.Samples, 2, input.Fragment2.PrimitiveID);
				if (input.Fragment3.Samples.AnyCovered)
					yield return GetPixel(input.Fragment3.X, input.Fragment3.Y, input.Fragment3.Samples, 3, input.Fragment3.PrimitiveID);
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