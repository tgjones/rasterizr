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
			try
			{
				foreach (var input in inputs)
				{
					input.Position = ToScreenCoordinates(input.Position);
					outputs.Add(input);
				}
			}
			finally
			{
				outputs.CompleteAdding();
			}
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