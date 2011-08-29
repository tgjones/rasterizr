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
			foreach (var input in GetVertices())
				outputs.Add(input);
			outputs.CompleteAdding();
		}

		private IEnumerable GetVertices()
		{
			if (Indices == null)
				return Vertices;
			return GetIndexedVertices();
		}

		private IEnumerable GetIndexedVertices()
		{
			return Indices.Select(t => Vertices[t]);
		}
	}
}