using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Meshellator;
using Nexus;
using Nexus.Graphics.Cameras;
using Rasterizr.Meshellator;
using Rasterizr.OutputMerger;
using Rasterizr.ShaderStages.Core;
using Rasterizr.ShaderStages.PixelShader;
using Rasterizr.ShaderStages.VertexShader;

namespace Rasterizr.WpfExamples
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			var outputImage = new WriteableBitmap((int)ImageViewport.Width, (int)ImageViewport.Height,
				96, 96, PixelFormats.Bgra32, null);

			var device = new RasterizrDevice((int)ImageViewport.Width, (int)ImageViewport.Height);
			var imageRenderer = new SwapChain(outputImage, (int)ImageViewport.Width, (int)ImageViewport.Height, 1);

			ImageViewport.Source = outputImage;

			device.RenderPipeline.OutputMerger.RenderTarget = new RenderTargetView(imageRenderer.GetBuffer());

			//Scene scene = MeshellatorLoader.ImportFromFile(@"Assets\85-nissan-fairlady.3ds");
			//var model = ModelLoader.FromScene(device, scene);

			//device.ClearDepthBuffer(1);
			//device.ClearRenderTarget(ColorsF.Green);

			//Camera camera = PerspectiveCamera.CreateFromBounds(scene.Bounds, MathUtility.PI_OVER_4);
			//foreach (ModelMesh mesh in model.Meshes)
			//{
			//    var effect = (BasicEffect) mesh.Effect;
			//    effect.View = camera.GetViewMatrix();
			//    effect.Projection = camera.GetProjectionMatrix(device.RenderPipeline.Rasterizer.Viewport.AspectRatio);
			//}
			//model.Draw();

			device.ClearDepthBuffer(1);
			device.ClearRenderTarget(ColorsF.White);

			device.RenderPipeline.InputAssembler.PrimitiveTopology = InputAssembler.PrimitiveTopology.TriangleList;
			device.RenderPipeline.InputAssembler.Vertices = new[]
			{
				new VertexPositionColor
				{
					Position = new Point3D(-1, 0, 0),
					Color = ColorsF.Red
				},
				new VertexPositionColor
				{
					Position = new Point3D(1, 0, 0),
					Color = ColorsF.Red
				},
				new VertexPositionColor
				{
					Position = new Point3D(0, 1, 0),
					Color = ColorsF.Red
				}
			};

			device.RenderPipeline.VertexShader.VertexShader = new TestVertexShader
			{
				Projection = Matrix3D.CreatePerspectiveFieldOfView(MathUtility.PI_OVER_4, 
					device.RenderPipeline.Rasterizer.Viewport.AspectRatio, 1.0f, 10.0f),
				View = Matrix3D.CreateLookAt(new Point3D(0, 0, 5), 
					Vector3D.Forward, Vector3D.Up)
			};
			device.RenderPipeline.PixelShader.PixelShader = new TestPixelShader();

			device.Draw();

			imageRenderer.Present();
		}

		private struct TestVertexColor : IVertexShaderOutput
		{
			public Point4D Position { get; set; }

			[Semantic(Semantics.Color)]
			public ColorF Color;
		}

		private class TestVertexShader : VertexShaderBase<VertexPositionColor, TestVertexColor>
		{
			public Matrix3D Projection { get; set; }
			public Matrix3D View { get; set; }

			public override TestVertexColor Execute(VertexPositionColor vertexShaderInput)
			{
				return new TestVertexColor
				{
					Position = (View * Projection).Transform(vertexShaderInput.Position.ToHomogeneousPoint3D()),
					Color = vertexShaderInput.Color
				};
			}
		}

		private class TestPixelShader : PixelShaderBase<TestVertexColor>
		{
			public override ColorF Execute(TestVertexColor pixelShaderInput)
			{
				return pixelShaderInput.Color;
			}
		}
	}
}