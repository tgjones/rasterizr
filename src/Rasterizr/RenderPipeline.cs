using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Nexus;
using Rasterizr.PipelineStages.InputAssembler;
using Rasterizr.PipelineStages.OutputMerger;
using Rasterizr.PipelineStages.Rasterizer;
using Rasterizr.PipelineStages.ShaderStages.GeometryShader;
using Rasterizr.PipelineStages.ShaderStages.PixelShader;
using Rasterizr.PipelineStages.ShaderStages.VertexShader;

namespace Rasterizr
{
	public class RenderPipeline
	{
		public const int MaxVertexAttributes = 16;

		#region Fields

		private readonly List<IVertex> _vertexShaderInputs;
		private readonly List<IVertexShaderOutput> _vertexShaderOutputs;
		private readonly List<TrianglePrimitive> _primitiveAssemblyOutputs;
		private readonly List<IVertexShaderOutput> _geometryShaderOutputs;
		private readonly List<Fragment> _fragments;
		private readonly List<Pixel> _pixels;

		#endregion

		public InputAssemblerStage InputAssembler { get; private set; }
		public VertexShaderStage VertexShader { get; private set; }
		public GeometryShaderStage GeometryShader { get; private set; }
		public RasterizerStage Rasterizer { get; private set; }
		public PixelShaderStage PixelShader { get; private set; }
		public OutputMergerStage OutputMerger { get; set; }

		public RenderPipeline(Viewport3D viewport)
		{
			InputAssembler = new InputAssemblerStage();
			VertexShader = new VertexShaderStage();
			GeometryShader = new GeometryShaderStage();
			
			PixelShader = new PixelShaderStage(viewport);
			OutputMerger = new OutputMergerStage();

			Rasterizer = new RasterizerStage(viewport, PixelShader, OutputMerger);

			_vertexShaderInputs = new List<IVertex>();
			_vertexShaderOutputs = new List<IVertexShaderOutput>();
			_primitiveAssemblyOutputs = new List<TrianglePrimitive>();
			_geometryShaderOutputs = new List<IVertexShaderOutput>();
			_fragments = new List<Fragment>(viewport.Width * viewport.Height);
			_pixels = new List<Pixel>(viewport.Width * viewport.Height);
		}

		public void Draw()
		{
			_vertexShaderInputs.Clear();
			_vertexShaderOutputs.Clear();
			_primitiveAssemblyOutputs.Clear();
			_geometryShaderOutputs.Clear();
			_fragments.Clear();
			_pixels.Clear();

			InputAssembler.Process(_vertexShaderInputs);
			Log(_vertexShaderInputs, "Input assembler");

			VertexShader.Process(_vertexShaderInputs, _vertexShaderOutputs);
			Log(_vertexShaderOutputs, "Vertex shader");

			GeometryShader.Process(_vertexShaderOutputs, _geometryShaderOutputs);
			Log(_geometryShaderOutputs, "Geometry shader");

			// New rasterizer: We are supplied the triangle coordinates in
			// screen space. Calculate bounding rect, and walk through every
			// sample, using edge equations to determine coverage.
			// Once coverage is determined, interpolate vertex attributes for each
			// pixel, using either pixel centre or centroid sampling as required.
			// Do this for 2x2 pixels at a time so that derivatives can be
			// calculated.

			Rasterizer.Process(_geometryShaderOutputs, _fragments);
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
	}
}