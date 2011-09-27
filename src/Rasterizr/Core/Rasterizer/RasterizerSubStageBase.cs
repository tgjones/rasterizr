using System.Collections.Generic;
using Rasterizr.Core.ShaderCore;

namespace Rasterizr.Core.Rasterizer
{
	public abstract class RasterizerSubStageBase
	{
		public abstract IEnumerable<TransformedVertex> Process(IEnumerable<TransformedVertex> inputs);
	}
}