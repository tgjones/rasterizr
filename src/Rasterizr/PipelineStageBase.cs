using System.Collections.Concurrent;

namespace Rasterizr
{
	public abstract class PipelineStageBase<TInput, TOutput>
	{
		public abstract void Run(BlockingCollection<TInput> inputs, BlockingCollection<TOutput> outputs);
	}
}