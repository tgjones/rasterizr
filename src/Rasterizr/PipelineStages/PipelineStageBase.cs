using System.Collections.Concurrent;

namespace Rasterizr.PipelineStages
{
	public abstract class PipelineStageBase<TInput, TOutput>
	{
		public abstract void Run(BlockingCollection<TInput> inputs, BlockingCollection<TOutput> outputs);
	}
}