using System;
using System.Collections.Concurrent;
using Nexus;
using Rasterizr.ShaderStages.VertexShader;

namespace Rasterizr.Rasterizer
{
	public class ScreenMapperSubStage : RasterizerSubStageBase<IVertexShaderOutput, IVertexShaderOutput>
	{
		public Viewport3D Viewport { get; set; }

		public override void Run(BlockingCollection<IVertexShaderOutput> inputs, BlockingCollection<IVertexShaderOutput> outputs)
		{
			foreach (var input in inputs)
			{
				input.Position = ToScreenCoordinates(input.Position);
				outputs.Add(input);
			}
			outputs.CompleteAdding();
		}

		private Point4D ToScreenCoordinates(Point4D position)
		{
			// Move to range [0,1]
			position.X = (position.X + 1f) / 2.0f;
			position.Y = ((position.Y * -1) + 1f) / 2.0f;

			// Sanity check.
			if (position.X > 1 || position.Y > 1 || position.X < 0 || position.Y < 0)
				throw new ArgumentException("Invalid position", "position");

			if (position.Z < 0)
				throw new ArgumentException("Invalid position", "position");

			// pra coordenadas de tela
			position.X *= (Viewport.Width - 1);
			position.Y *= (Viewport.Height - 1);
			return position;
		}
	}
}