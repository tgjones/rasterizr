using System.Collections.Generic;
using Rasterizr.Pipeline.InputAssembler;

namespace Rasterizr.Pipeline.Rasterizer
{
	public class RasterizerStage
	{
		public RasterizerState State { get; set; }

		public void SetViewports(params Viewport[] viewports)
		{

		}

		internal object Execute(IEnumerable<InputAssemblerPrimitiveOutput> primitiveStream)
		{
			throw new System.NotImplementedException();
		}
	}
}