using System;
using System.Collections.Generic;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.Rasterizer.Culling;
using Rasterizr.Pipeline.Rasterizer.Primitives;
using Rasterizr.Util;
using SlimShader;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Pipeline.Rasterizer
{
	public class RasterizerStage
	{
        public event DiagnosticEventHandler SettingState;
        public event DiagnosticEventHandler SettingViewports;

		private Viewport[] _viewports;

	    private RasterizerState _state;

	    public RasterizerState State
	    {
	        get { return _state; }
	        set
	        {
                DiagnosticUtilities.RaiseEvent(this, SettingState, DiagnosticUtilities.GetID(value));
	            _state = value;
	        }
	    }

        public Func<int, int, bool> FragmentFilter { get; set; }

	    public RasterizerStage(Device device)
		{
			State = new RasterizerState(device, RasterizerStateDescription.Default);
		}

		public void SetViewports(params Viewport[] viewports)
		{
            DiagnosticUtilities.RaiseEvent(this, SettingViewports, viewports);
			_viewports = viewports;
		}

		internal IEnumerable<FragmentQuad> Execute(
            IEnumerable<InputAssemblerPrimitiveOutput> inputs, 
            PrimitiveTopology primitiveTopology, 
            OutputSignatureChunk previousStageOutputSignature, 
            BytecodeContainer pixelShader,
            int multiSampleCount)
		{
            // TODO: Allow selection of different viewport.
		    var viewport = _viewports[0];

		    var outputInputBindings = ShaderOutputInputBindings.FromShaderSignatures(
		        previousStageOutputSignature, pixelShader);

			var rasterizer = PrimitiveRasterizerFactory.CreateRasterizer(
                primitiveTopology, State.Description, multiSampleCount,
                outputInputBindings, ref viewport, FragmentFilter);

            foreach (var primitive in inputs)
			{
                // Frustum culling.
			    if (ViewportCuller.ShouldCullTriangle(primitive.Vertices))
			        continue;

                // TODO: Clipping.
                // http://simonstechblog.blogspot.tw/2012/04/software-rasterizer-part-2.html#softwareRasterizerDemo

                // Perspective divide.
			    for (int i = 0; i < primitive.Vertices.Length; i++)
			        PerspectiveDivide(ref primitive.Vertices[i].Position);

			    // Backface culling.
			    if (State.Description.CullMode != CullMode.None && rasterizer.ShouldCull(primitive.Vertices))
			        continue;

                // Transform from clip space to screen space.
			    for (int i = 0; i < primitive.Vertices.Length; i++)
                    viewport.MapClipSpaceToScreenSpace(ref primitive.Vertices[i].Position);

                // Rasterize.
				foreach (var fragmentQuad in rasterizer.Rasterize(primitive))
					yield return fragmentQuad;
			}
		}

        private static void PerspectiveDivide(ref Number4 position)
		{
			position.X /= position.W;
			position.Y /= position.W;
			position.Z /= position.W;
		}
	}
}