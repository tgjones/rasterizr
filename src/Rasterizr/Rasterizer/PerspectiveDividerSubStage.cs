using System.Collections.Generic;
using Rasterizr.ShaderCore.VertexShader;

namespace Rasterizr.Rasterizer
{
	public class PerspectiveDividerSubStage : RasterizerSubStageBase
	{
		public void Process(List<IVertexShaderOutput> vertices)
		{
			for (int i = 0; i < vertices.Count; i++)
			{
				var input = vertices[i];
				var position = input.Position;
				position.X /= position.W;
				position.Y /= position.W;
				position.Z /= position.W;
				vertices[i].Position = position;
			}
		}
	}
}