using System.Collections.Generic;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages
{
	public abstract class PipelineStageBase<TInput, TOutput>
	{
		public abstract void Process(IList<TInput> inputs, IList<TOutput> outputs);
	}
}