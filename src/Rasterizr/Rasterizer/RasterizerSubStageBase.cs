using System.Collections.Generic;
using Rasterizr.ShaderCore;

namespace Rasterizr.Rasterizer
{
	public abstract class RasterizerSubStageBase
	{
		public abstract IEnumerable<TransformedVertex> Process(IEnumerable<TransformedVertex> inputs);
	}
}