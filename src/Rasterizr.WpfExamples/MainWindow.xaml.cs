using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Meshellator;
using Nexus;
using Nexus.Graphics.Cameras;
using Rasterizr.Meshellator;
using Rasterizr.OutputMerger;
using Rasterizr.ShaderCore;
using Rasterizr.ShaderCore.PixelShader;
using Rasterizr.ShaderCore.VertexShader;

namespace Rasterizr.WpfExamples
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private RasterizrDevice _device;
		private SwapChain _imageRenderer;
		private float _angle;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			var outputImage = new WriteableBitmap((int)ImageViewport.Width, (int)ImageViewport.Height,
				96, 96, PixelFormats.Bgra32, null);

			_device = new RasterizrDevice();
			_imageRenderer = new SwapChain(outputImage, (int)ImageViewport.Width, (int)ImageViewport.Height, 1);

			ImageViewport.Source = outputImage;

			_device.Rasterizer.Viewport = new Viewport3D(0, 0, (int)ImageViewport.Width, (int)ImageViewport.Height);
			_device.OutputMerger.RenderTarget = new RenderTargetView(_imageRenderer.GetBuffer());

			//Scene scene = MeshellatorLoader.ImportFromFile(@"Assets\85-nissan-fairlady.3ds");
			//var model = ModelLoader.FromScene(device, scene);

			//device.ClearDepthBuffer(1);
			//device.ClearRenderTarget(ColorsF.Green);

			//Camera camera = PerspectiveCamera.CreateFromBounds(scene.Bounds, MathUtility.PI_OVER_4);
			//foreach (ModelMesh mesh in model.Meshes)
			//{
			//    var effect = (BasicEffect)mesh.Effect;
			//    effect.View = camera.GetViewMatrix();
			//    effect.Projection = camera.GetProjectionMatrix(device.Rasterizer.Viewport.AspectRatio);
			//}
			//model.Draw();

			_device.InputAssembler.PrimitiveTopology = InputAssembler.PrimitiveTopology.TriangleList;
			_device.InputAssembler.Vertices = new[]
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
				},
				new VertexPositionColor
				{
					Position = new Point3D(0, 0, -1),
					Color = ColorsF.Blue
				},
				new VertexPositionColor
				{
					Position = new Point3D(2, 0, -1),
					Color = ColorsF.Blue
				},
				new VertexPositionColor
				{
					Position = new Point3D(1, 1, -1),
					Color = ColorsF.Blue
				}
			};

			_device.VertexShader.VertexShader = new TestVertexShader
			{
				Projection = Matrix3D.CreatePerspectiveFieldOfView(MathUtility.PI_OVER_4,
					_device.Rasterizer.Viewport.AspectRatio, 1.0f, 10.0f),
				View = Matrix3D.CreateLookAt(new Point3D(0, 0, 5),
					Vector3D.Forward, Vector3D.Up)
			};
			_device.PixelShader.PixelShader = new TestPixelShader();

			CompositionTarget.Rendering += (sender2, e2) => Draw();
		}

		private void Draw()
		{
			_device.ClearDepthBuffer(1);
			_device.ClearRenderTarget(ColorsF.White);

			_angle += 0.01f;
			((TestVertexShader) _device.VertexShader.VertexShader).View = 
				Matrix3D.CreateLookAt(new Point3D(0, 0, 5), Vector3D.Forward, Vector3D.Up) * 
				Matrix3D.CreateRotationY(_angle);

			_device.Draw();

			_imageRenderer.Present();
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