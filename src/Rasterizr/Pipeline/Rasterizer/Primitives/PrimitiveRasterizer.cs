using System;
using System.Collections.Generic;
using Rasterizr.Math;
using Rasterizr.Pipeline.InputAssembler;

namespace Rasterizr.Pipeline.Rasterizer.Primitives
{
	internal abstract class PrimitiveRasterizer
	{
	    protected readonly RasterizerStateDescription RasterizerState;
	    protected readonly int MultiSampleCount;
	    protected readonly ShaderOutputInputBindings OutputInputBindings;
        protected readonly Box2D ScreenBounds;
	    protected Func<int, int, bool> FragmentFilter;

	    protected PrimitiveRasterizer(
            RasterizerStateDescription rasterizerState, int multiSampleCount,
            ShaderOutputInputBindings outputInputBindings,
            ref Viewport viewport,
            Func<int, int, bool> fragmentFilter)
	    {
            RasterizerState = rasterizerState;
            MultiSampleCount = multiSampleCount;
	        OutputInputBindings = outputInputBindings;
            ScreenBounds = new Box2D(
                viewport.TopLeftX, viewport.TopLeftY,
                viewport.TopLeftX + viewport.Width, 
                viewport.TopLeftY + viewport.Height);
	        FragmentFilter = fragmentFilter;
	    }

        public abstract bool ShouldCull(VertexShader.VertexShaderOutput[] vertices);

		public abstract IEnumerable<FragmentQuad> Rasterize(InputAssemblerPrimitiveOutput primitive);

        protected Point GetSamplePosition(int x, int y, int sampleIndex)
		{
            return MultiSamplingUtility.GetSamplePosition(MultiSampleCount, x, y, sampleIndex);
		}
	}
}