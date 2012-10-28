using System.Collections.Generic;

namespace Rasterizr
{
	public abstract class PipelineStageBase
	{
		
	}

	public abstract class PipelineStageBase<TInput, TOutput> : PipelineStageBase
	{
		public abstract IEnumerable<TOutput> Run(IEnumerable<TInput> inputs);
	}
}