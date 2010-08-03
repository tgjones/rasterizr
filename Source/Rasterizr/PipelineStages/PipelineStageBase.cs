using System.Collections.Generic;

namespace Rasterizr.PipelineStages
{
	public abstract class PipelineStageBase<TInput, TOutput>
	{
		public abstract void Process(IList<TInput> inputs, IList<TOutput> outputs);
	}
}