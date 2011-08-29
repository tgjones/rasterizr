using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using Nexus;

namespace Rasterizr.PipelineStages.InputAssembler
{
	public class InputAssemblerStage
	{
		public InputLayout InputLayout { get; set; }
		public PrimitiveTopology PrimitiveTopology { get; set; }

		public IList Vertices { get; set; }
		public Int32Collection Indices { get; set; }

		public void Run(BlockingCollection<object> outputs)
		{
			switch (PrimitiveTopology)
			{
				case PrimitiveTopology.TriangleList:
					foreach (var vertex in GetVertices())
						outputs.Add(vertex);
					break;
				case PrimitiveTopology.TriangleStrip:
					var vertices = GetVertices();
					bool even = true;
					for (int i = 0; i <= vertices.Count - 3; i++)
					{
						if (even)
						{
							outputs.Add(Vertices[i + 0]);
							outputs.Add(Vertices[i + 1]);
							outputs.Add(Vertices[i + 2]);
						}
						else
						{
							outputs.Add(Vertices[i + 0]);
							outputs.Add(Vertices[i + 2]);
							outputs.Add(Vertices[i + 1]);
						}
						even = !even;
					}
					break;
				default:
					throw new NotSupportedException();
			}
			outputs.CompleteAdding();
		}

		private IList GetVertices()
		{
			if (Indices != null)
				return Indices.Select(t => Vertices[t]).ToList();
			return Vertices;
		}
	}
}