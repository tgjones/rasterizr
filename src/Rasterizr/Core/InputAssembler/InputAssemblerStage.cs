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

		public IEnumerable<InputAssemblerOutput> Run(int vertexCount, int startVertexLocation)
		{
			return RunInternal(GetVertices(vertexCount, startVertexLocation));
		}

		public IEnumerable<InputAssemblerOutput> RunIndexed(int indexCount, int startIndexLocation, int baseVertexLocation)
		{
			return RunInternal(GetVerticesIndexed(indexCount, startIndexLocation, baseVertexLocation));
		}

		private IEnumerable<InputAssemblerOutput> RunInternal(IEnumerable<InputVertex> vertices)
		{
			switch (PrimitiveTopology)
			{
				case PrimitiveTopology.TriangleList:
					foreach (var vertex in vertices)
						yield return new InputAssemblerOutput(vertex.VertexID, vertex.VertexID/3, vertex.Vertex);
					break;
				case PrimitiveTopology.TriangleStrip:
					var verticesList = vertices.ToList();
					bool even = true;
					for (int i = 0; i <= verticesList.Count - 3; i++)
					{
						if (even)
						{
							yield return new InputAssemblerOutput(verticesList[i + 0].VertexID, i, verticesList[i + 0].Vertex);
							yield return new InputAssemblerOutput(verticesList[i + 1].VertexID, i, verticesList[i + 1].Vertex);
							yield return new InputAssemblerOutput(verticesList[i + 2].VertexID, i, verticesList[i + 2].Vertex);
						}
						else
						{
							yield return new InputAssemblerOutput(verticesList[i + 0].VertexID, i, verticesList[i + 0].Vertex);
							yield return new InputAssemblerOutput(verticesList[i + 2].VertexID, i, verticesList[i + 2].Vertex);
							yield return new InputAssemblerOutput(verticesList[i + 1].VertexID, i, verticesList[i + 1].Vertex);
						}
						even = !even;
					}
					break;
				default:
					throw new NotSupportedException();
			}
		}

		private IEnumerable<InputVertex> GetVertices(int vertexCount, int startVertexLocation)
		{
			for (int i = startVertexLocation; i < startVertexLocation + vertexCount; i++)
				yield return new InputVertex(i, (Vertices != null && Vertices.Count > i) ? Vertices[i] : null);
		}

		private IEnumerable<InputVertex> GetVerticesIndexed(int indexCount, int startIndexLocation, int baseVertexLocation)
		{
			for (int i = startIndexLocation; i < startIndexLocation + indexCount; i++)
			{
				int indexValue = Indices[i];
				yield return new InputVertex(indexValue, 
					Vertices[Indices[i] + baseVertexLocation]);
			}
		}

		private struct InputVertex
		{
			public readonly int VertexID;
			public readonly object Vertex;

			public InputVertex(int vertexID, object vertex)
			{
				VertexID = vertexID;
				Vertex = vertex;
			}
		}
	}
}