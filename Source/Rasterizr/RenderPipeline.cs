using System.Collections.Generic;
using Rasterizr.PipelineStages.InputAssembler;
using Rasterizr.PipelineStages.OutputMerger;
using Rasterizr.PipelineStages.PerspectiveDivide;
using Rasterizr.PipelineStages.Rasterizer;
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
		private readonly List<ScreenSpaceVertex> _screenSpaceVertices;
		private readonly List<Triangle> _triangles;
		private readonly List<Fragment> _fragments;
		private readonly List<Pixel> _pixels;

		#endregion

		public InputAssemblerStage InputAssembler { get; private set; }
		public VertexShaderStage VertexShader { get; private set; }
		public PerspectiveDivideStage PerspectiveDivide { get; private set; }
		public TriangleSetupStage TriangleSetup { get; private set; }
		public RasterizerStage Rasterizer { get; private set; }
		public PixelShaderStage PixelShader { get; private set; }
		public OutputMergerStage OutputMerger { get; set; }

		public RenderPipeline(Viewport3D viewport)
		{
			InputAssembler = new InputAssemblerStage();
			VertexShader = new VertexShaderStage();
			PerspectiveDivide = new PerspectiveDivideStage(viewport);
			TriangleSetup = new TriangleSetupStage();
			Rasterizer = new RasterizerStage(viewport);
			PixelShader = new PixelShaderStage(viewport);
			OutputMerger = new OutputMergerStage();

			_vertexShaderInputs = new List<VertexShaderInput>();
			_vertexShaderOutputs = new List<VertexShaderOutput>();
			_screenSpaceVertices = new List<ScreenSpaceVertex>();
			_triangles = new List<Triangle>();
			_fragments = new List<Fragment>(viewport.Width * viewport.Height);
			_pixels = new List<Pixel>(viewport.Width * viewport.Height);
		}

		public void Clear()
		{
			OutputMerger.RenderTarget.Clear();
		}

		public void Draw()
		{
			_vertexShaderInputs.Clear();
			_vertexShaderOutputs.Clear();
			_screenSpaceVertices.Clear();
			_triangles.Clear();
			_fragments.Clear();
			_pixels.Clear();

			InputAssembler.Process(_vertexShaderInputs);
			VertexShader.Process(_vertexShaderInputs, _vertexShaderOutputs);
			PerspectiveDivide.Process(_vertexShaderOutputs, _screenSpaceVertices);
			TriangleSetup.Process(_screenSpaceVertices, _triangles);
			Rasterizer.Process(_triangles, _fragments);
			PixelShader.Process(_fragments, _pixels);
			OutputMerger.Process(_pixels);
		}
	}
}