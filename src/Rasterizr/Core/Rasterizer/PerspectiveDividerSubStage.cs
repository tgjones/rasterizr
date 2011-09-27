using System.Collections.Generic;
using Rasterizr.Core.ShaderCore.VertexShader;

namespace Rasterizr.Core.Rasterizer
{
	public class PerspectiveDividerSubStage : RasterizerSubStageBase
	{
		public IEnumerable<IVertexShaderOutput> Process(IEnumerable<IVertexShaderOutput> inputs)
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