using System.Collections.Generic;
using Nexus;
using Rasterizr.ShaderStages.VertexShader;

namespace Rasterizr.Rasterizer
{
	public class ScreenMapperSubStage : RasterizerSubStageBase
	{
		public Viewport3D Viewport { get; set; }

		public void Process(List<IVertexShaderOutput> vertices)
		{
			for (int i = 0; i < vertices.Count; i++)
				vertices[i].Position = ToScreenCoordinates(vertices[i].Position);
		}

		/// <summary>
		/// Formulae from http://msdn.microsoft.com/en-us/library/bb205126(v=vs.85).aspx
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		private Point4D ToScreenCoordinates(Point4D position)
		{
			position.X = (position.X + 1) * Viewport.Width * 0.5f + Viewport.X;
			position.Y = (1 - position.Y) * Viewport.Height * 0.5f + Viewport.Y;
			position.Z = Viewport.MinDepth + position.Z * (Viewport.MaxDepth - Viewport.MinDepth);
			return position;
		}
	}
}