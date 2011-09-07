using System.Collections.Concurrent;

namespace Rasterizr
{
	public abstract class PipelineStageBase
	{
		public virtual void Validate()
		{

		}
	}

	public abstract class PipelineStageBase<TInput, TOutput> : PipelineStageBase
	{
		public abstract void Run(BlockingCollection<TInput> inputs, BlockingCollection<TOutput> outputs);
	}
}