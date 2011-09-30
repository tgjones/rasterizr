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

		public IEnumerable<object> Run(int vertexCount, int startVertexLocation)
		{
			return RunInternal(GetVertices(vertexCount, startVertexLocation));
		}

		public IEnumerable<object> RunIndexed(int indexCount, int startIndexLocation, int baseVertexLocation)
		{
			return RunInternal(GetVerticesIndexed(indexCount, startIndexLocation, baseVertexLocation));
		}

		private IEnumerable<object> RunInternal(IEnumerable<object> vertices)
		{
			switch (PrimitiveTopology)
			{
				case PrimitiveTopology.TriangleList:
					foreach (var vertex in vertices)
						yield return vertex;
					break;
				case PrimitiveTopology.TriangleStrip:
					var verticesList = vertices.ToList();
					bool even = true;
					for (int i = 0; i <= verticesList.Count - 3; i++)
					{
						if (even)
						{
							yield return verticesList[i + 0];
							yield return verticesList[i + 1];
							yield return verticesList[i + 2];
						}
						else
						{
							yield return verticesList[i + 0];
							yield return verticesList[i + 2];
							yield return verticesList[i + 1];
						}
						even = !even;
					}
					break;
				default:
					throw new NotSupportedException();
			}
		}

		private IEnumerable<object> GetVertices(int vertexCount, int startVertexLocation)
		{
			for (int i = startVertexLocation; i < startVertexLocation + vertexCount; i++)
				yield return Vertices[i];
		}

		private IEnumerable<object> GetVerticesIndexed(int indexCount, int startIndexLocation, int baseVertexLocation)
		{
			for (int i = startIndexLocation; i < startIndexLocation + indexCount; i++)
				yield return Vertices[Indices[i] + baseVertexLocation];
		}
	}
}