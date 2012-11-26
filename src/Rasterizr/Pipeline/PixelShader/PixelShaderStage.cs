using System.Collections.Generic;
using Rasterizr.Pipeline.Rasterizer;

namespace Rasterizr.Pipeline.PixelShader
{
	public class PixelShaderStage : CommonShaderStage<PixelShader>
	{
		internal IEnumerable<PixelShaderOutput> Execute(IEnumerable<Fragment> inputs)
		{
			foreach (var input in inputs)
			{
				yield return new PixelShaderOutput
				{

				};
			}
		}
	}

	internal struct PixelShaderOutput
	{
		
	}
}