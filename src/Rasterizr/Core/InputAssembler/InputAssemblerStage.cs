using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nexus;

namespace Rasterizr.Core.InputAssembler
{
	public class InputAssemblerStage : PipelineStageBase
	{
		public InputLayout InputLayout { get; set; }
		public PrimitiveTopology PrimitiveTopology { get; set; }

		public IList Vertices { get; set; }
		public Int32Collection Indices { get; set; }

		public IEnumerable<object> Run()
		{
			switch (PrimitiveTopology)
			{
				case PrimitiveTopology.TriangleList:
					foreach (var vertex in GetVertices())
						yield return vertex;
					break;
				case PrimitiveTopology.TriangleStrip:
					var vertices = GetVertices();
					bool even = true;
					for (int i = 0; i <= vertices.Count - 3; i++)
					{
						if (even)
						{
							yield return Vertices[i + 0];
							yield return Vertices[i + 1];
							yield return Vertices[i + 2];
						}
						else
						{
							yield return Vertices[i + 0];
							yield return Vertices[i + 2];
							yield return Vertices[i + 1];
						}
						even = !even;
					}
					break;
				default:
					throw new NotSupportedException();
			}
		}

		private IList GetVertices()
		{
			if (Indices != null)
				return Indices.Select(t => Vertices[t]).ToList();
			return Vertices;
		}
	}
}