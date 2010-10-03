using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Rasterizr.PipelineStages.InputAssembler;
using Rasterizr.PipelineStages.OutputMerger;
using Rasterizr.PipelineStages.PerspectiveDivide;
using Rasterizr.PipelineStages.PrimitiveAssembly;
using Rasterizr.PipelineStages.Rasterizer;
using Rasterizr.PipelineStages.ShaderStages.GeometryShader;
using Rasterizr.PipelineStages.ShaderStages.PixelShader;
using Rasterizr.PipelineStages.ShaderStages.VertexShader;
using Rasterizr.PipelineStages.TriangleSetup;

namespace Rasterizr
{
	public class RenderPipeline
	{
		public const int MaxVertexAttributes = 16;

		#region Fields

		private readonly List<VertexShaderInput> _vertexShaderInputs;
		private readonly List<VertexShaderOutput> _vertexShaderOutputs;
		private readonly List<TrianglePrimitive> _primitiveAssemblyOutputs;
		private readonly List<TrianglePrimitive> _geometryShaderOutputs;
		private readonly List<ScreenSpaceVertex> _screenSpaceVertices;
		private readonly List<Triangle> _triangles;
		private readonly List<Fragment> _fragments;
		private readonly List<Pixel> _pixels;

		#endregion

		public InputAssemblerStage InputAssembler { get; private set; }
		public VertexShaderStage VertexShader { get; private set; }
		public PrimitiveAssemblyStage PrimitiveAssembly { get; private set; }
		public GeometryShaderStage GeometryShader { get; private set; }
		public PerspectiveDivideStage PerspectiveDivide { get; private set; }
		public TriangleSetupStage TriangleSetup { get; private set; }
		public RasterizerStage Rasterizer { get; private set; }
		public PixelShaderStage PixelShader { get; private set; }
		public OutputMergerStage OutputMerger { get; set; }

		public RenderPipeline(Viewport3D viewport)
		{
			InputAssembler = new InputAssemblerStage();
			VertexShader = new VertexShaderStage();
			PrimitiveAssembly = new PrimitiveAssemblyStage();
			GeometryShader = new GeometryShaderStage();
			PerspectiveDivide = new PerspectiveDivideStage(viewport);
			TriangleSetup = new TriangleSetupStage();
			Rasterizer = new RasterizerStage(viewport);
			PixelShader = new PixelShaderStage(viewport);
			OutputMerger = new OutputMergerStage();

			_vertexShaderInputs = new List<VertexShaderInput>();
			_vertexShaderOutputs = new List<VertexShaderOutput>();
			_primitiveAssemblyOutputs = new List<TrianglePrimitive>();
			_geometryShaderOutputs = new List<TrianglePrimitive>();
			_screenSpaceVertices = new List<ScreenSpaceVertex>();
			_triangles = new List<Triangle>();
			_fragments = new List<Fragment>(viewport.Width * viewport.Height);
			_pixels = new List<Pixel>(viewport.Width * viewport.Height);
		}

		public void Clear()
		{
			OutputMerger.DepthBuffer.Clear(1);
			OutputMerger.RenderTarget.Clear();
		}

		public void Draw()
		{
			_vertexShaderInputs.Clear();
			_vertexShaderOutputs.Clear();
			_primitiveAssemblyOutputs.Clear();
			_geometryShaderOutputs.Clear();
			_screenSpaceVertices.Clear();
			_triangles.Clear();
			_fragments.Clear();
			_pixels.Clear();

			InputAssembler.Process(_vertexShaderInputs);
			Log(_vertexShaderInputs, "Input assembler");

			VertexShader.Process(_vertexShaderInputs, _vertexShaderOutputs);
			Log(_vertexShaderOutputs, "Vertex shader");

			PrimitiveAssembly.Process(_vertexShaderOutputs, _primitiveAssemblyOutputs);
			Log(_primitiveAssemblyOutputs, "Primitive assembly");

			GeometryShader.Process(_primitiveAssemblyOutputs, _geometryShaderOutputs);
			Log(_geometryShaderOutputs, "Geometry shader");

			// TODO: Culling

			// TODO: Clipping

			// New rasterizer: We are supplied the triangle coordinates in
			// screen space. Calculate bounding rect, and walk through every
			// sample, using edge equations to determine coverage.
			// Once coverage is determined, interpolate vertex attributes for each
			// pixel, using either pixel centre or centroid sampling as required.
			// Do this for 2x2 pixels at a time so that derivatives can be
			// calculated.

			PerspectiveDivide.Process(_geometryShaderOutputs, _screenSpaceVertices);
			Log(_screenSpaceVertices, "Perspective divide");

			TriangleSetup.Process(_screenSpaceVertices, _triangles);
			Log(_triangles, "Triangle setup");

			Rasterizer.Process(_triangles, _fragments);
			Log(_fragments, "Rasterizer");

			PixelShader.Process(_fragments, _pixels);
			Log(_pixels, "Pixel shader");

			OutputMerger.Process(_pixels);
			Log(_pixels, "Output merger");
		}

		private void Log(ICollection collection, string title)
		{
#if !SILVERLIGHT
			Trace.WriteLine("Outputs from " + title + ": " + collection.Count);
#endif
		}

		public void Present()
		{
			OutputMerger.RenderTarget.EndFrame();
		}
	}
}