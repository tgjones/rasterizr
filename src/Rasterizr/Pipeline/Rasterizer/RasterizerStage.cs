using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rasterizr.Diagnostics;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.Rasterizer.Culling;
using Rasterizr.Pipeline.Rasterizer.Primitives;
using SlimShader;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Pipeline.Rasterizer
{
	public class RasterizerStage
	{
		private readonly Device _device;
		private Viewport[] _viewports;

		private readonly TriangleRasterizer _triangleRasterizer;

		public RasterizerState State { get; set; }

		public RasterizerStage(Device device)
		{
			_device = device;
			_triangleRasterizer = new TriangleRasterizer();

			State = new RasterizerState(device, RasterizerStateDescription.Default);
		}

		public void SetViewports(params Viewport[] viewports)
		{
			_device.Loggers.BeginOperation(OperationType.RasterizerStageSetViewports, viewports);
			_viewports = viewports;
		}

		internal IEnumerable<FragmentQuad> Execute(
            IEnumerable<InputAssemblerPrimitiveOutput> inputs, 
            PrimitiveTopology primitiveTopology, 
            OutputSignatureChunk previousStageOutputSignature, 
            BytecodeContainer pixelShader,
            int multiSampleCount)
		{
			PrimitiveRasterizer rasterizer;
			switch (primitiveTopology)
			{
				case PrimitiveTopology.PointList:
					throw new NotImplementedException();
				case PrimitiveTopology.LineList:
				case PrimitiveTopology.LineStrip:
					throw new NotImplementedException();
				case PrimitiveTopology.TriangleList:
				case PrimitiveTopology.TriangleStrip:
					rasterizer = _triangleRasterizer;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

		    rasterizer.OutputInputBindings = ShaderOutputInputBindings.FromShaderSignatures(
		        previousStageOutputSignature, pixelShader);

		    var vp = _viewports[0];
		    rasterizer.ScreenBounds = new Math.Box2D(vp.TopLeftX, vp.TopLeftY, vp.TopLeftX + vp.Width, vp.TopLeftY + vp.Height);

		    rasterizer.RasterizerState = State.Description;
			rasterizer.IsMultiSamplingEnabled = State.Description.IsMultisampleEnabled;
			rasterizer.MultiSampleCount = multiSampleCount;
			rasterizer.FillMode = State.Description.FillMode;

            foreach (var primitive in inputs)
			{
                // Cull primitives that are outside view frustum.
			    if (ViewportCuller.ShouldCullTriangle(primitive.Vertices))
			        continue;

                // TODO: Clipping.
                // http://simonstechblog.blogspot.tw/2012/04/software-rasterizer-part-2.html#softwareRasterizerDemo

			    for (int i = 0; i < primitive.Vertices.Length; i++)
			        PerspectiveDivide(ref primitive.Vertices[i].Position);

                // Backface culling.
			    if (State.Description.CullMode != CullMode.None && rasterizer.ShouldCull(primitive.Vertices))
			        continue;

			    for (int i = 0; i < primitive.Vertices.Length; i++)
			        ToScreenCoordinates(ref primitive.Vertices[i].Position);

			    rasterizer.Primitive = primitive;
				foreach (var fragmentQuad in rasterizer.Rasterize())
					yield return fragmentQuad;
			}
		}

        private static void PerspectiveDivide(ref Number4 position)
		{
			position.X /= position.W;
			position.Y /= position.W;
			position.Z /= position.W;
		}

		/// <summary>
		/// Formulae from http://msdn.microsoft.com/en-us/library/bb205126(v=vs.85).aspx
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
        private void ToScreenCoordinates(ref Number4 position)
		{
			var viewport = _viewports[0];
			position.X = (position.X + 1) * viewport.Width * 0.5f + viewport.TopLeftX;
			position.Y = (1 - position.Y) * viewport.Height * 0.5f + viewport.TopLeftY;
			position.Z = viewport.MinDepth + position.Z * (viewport.MaxDepth - viewport.MinDepth);
		}
	}
}