using System.Collections.Generic;
using Rasterizr.ShaderCore;

namespace Rasterizr.Rasterizer
{
	public class PerspectiveDividerSubStage : RasterizerSubStageBase
	{
		public override IEnumerable<TransformedVertex> Process(IEnumerable<TransformedVertex> inputs)
		{
			foreach (var vertex in inputs)
			{
				var position = vertex.Position;
				position.X /= position.W;
				position.Y /= position.W;
				position.Z /= position.W;
				vertex.Position = position;
				yield return vertex;
			}
		}
	}
}