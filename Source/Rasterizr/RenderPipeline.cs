using System.Collections.Generic;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.InputAssembler;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.OutputMerger;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.PerspectiveDivide;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.PixelShader;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.Rasterizer;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.TriangleSetup;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer
{
	public class RenderPipeline
	{
		public InputAssemblerStage InputAssembler { get; private set; }
		public VertexShaderStage VertexShader { get; private set; }
		public PerspectiveDivideStage PerspectiveDivide { get; private set; }
		public TriangleSetupStage TriangleSetup { get; private set; }
		public RasterizerStage Rasterizer { get; private set; }
		public PixelShaderStage PixelShader { get; private set; }
		public OutputMergerStage OutputMerger { get; set; }

		public RenderPipeline()
		{
			InputAssembler = new InputAssemblerStage();
			VertexShader = new VertexShaderStage();
			PerspectiveDivide = new PerspectiveDivideStage();
			TriangleSetup = new TriangleSetupStage();
			Rasterizer = new RasterizerStage();
			PixelShader = new PixelShaderStage();
			OutputMerger = new OutputMergerStage();
		}

		public void Clear()
		{
			OutputMerger.RenderTarget.Clear();
		}

		public void Draw()
		{
			List<VertexShaderInput> vertexShaderInputs = new List<VertexShaderInput>();
			InputAssembler.Process(vertexShaderInputs);

			List<VertexShaderOutput> vertexShaderOutputs = new List<VertexShaderOutput>();
			VertexShader.Process(vertexShaderInputs, vertexShaderOutputs);

			List<ScreenSpaceVertex> screenSpaceVertices = new List<ScreenSpaceVertex>();
			PerspectiveDivide.Process(vertexShaderOutputs, screenSpaceVertices);

			List<Triangle> triangles = new List<Triangle>();
			TriangleSetup.Process(screenSpaceVertices, triangles);

			List<Fragment> fragments = new List<Fragment>();
			Rasterizer.Process(triangles, fragments);

			List<Pixel> pixels = new List<Pixel>();
			PixelShader.Process(fragments, pixels);

			OutputMerger.Process(pixels);
		}
	}
}