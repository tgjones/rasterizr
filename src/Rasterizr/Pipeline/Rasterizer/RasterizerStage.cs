using System;
using System.Collections.Generic;
using Rasterizr.Math;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.Rasterizer.Primitives;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Pipeline.Rasterizer
{
	public class RasterizerStage
	{
		private Viewport[] _viewports;

		private readonly TriangleRasterizer _triangleRasterizer;

		public RasterizerState State { get; set; }

		public RasterizerStage(Device device)
		{
			_triangleRasterizer = new TriangleRasterizer();

			State = new RasterizerState(device, RasterizerStateDescription.Default);
		}

		public void SetViewports(params Viewport[] viewports)
		{
			_viewports = viewports;
		}

		internal IEnumerable<FragmentQuad> Execute(IEnumerable<InputAssemblerPrimitiveOutput> inputs,
			PrimitiveTopology primitiveTopology, OutputSignatureChunk previousStageOutputSignature,
			InputSignatureChunk pixelShaderInputSignature, int multiSampleCount)
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

			rasterizer.PreviousStageOutputSignature = previousStageOutputSignature;
			rasterizer.PixelShaderInputSignture = pixelShaderInputSignature;
			rasterizer.MultiSampleCount = multiSampleCount;
			rasterizer.FillMode = State.Description.FillMode;

			foreach (var primitive in inputs)
			{
				for (int i = 0; i < primitive.Vertices.Length; i++)
				{
					PerspectiveDivide(ref primitive.Vertices[i].Position);
					ToScreenCoordinates(ref primitive.Vertices[i].Position);
				}

				rasterizer.Primitive = primitive;
				foreach (var fragmentQuad in rasterizer.Rasterize())
					yield return fragmentQuad;
			}
		}

		private static void PerspectiveDivide(ref Vector4 position)
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
		private void ToScreenCoordinates(ref Vector4 position)
		{
			var viewport = _viewports[0];
			position.X = (position.X + 1) * viewport.Width * 0.5f + viewport.TopLeftX;
			position.Y = (1 - position.Y) * viewport.Height * 0.5f + viewport.TopLeftY;
			position.Z = viewport.MinDepth + position.Z * (viewport.MaxDepth - viewport.MinDepth);
		}
	}
}