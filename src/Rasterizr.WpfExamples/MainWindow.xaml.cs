using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Meshellator;
using Nexus;
using Nexus.Graphics.Cameras;
//using Oriel;
//using Oriel.WarpRenderer;
using Rasterizr.Meshellator;
using Rasterizr.PipelineStages.OutputMerger;
using Rasterizr.PipelineStages.ShaderStages.Core;

namespace Rasterizr.WpfExamples
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Model _model;
		private RasterizrDevice _device;
		//private WarpDevice _warpDevice;
		//private BasicEffect _basicEffect;

		private SwapChain _imageRenderer;

		private float _angle;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			WriteableBitmap outputImage = new WriteableBitmap((int)ImageViewport.Width, (int)ImageViewport.Height,
				96, 96, PixelFormats.Bgra32, null);

			_device = new RasterizrDevice((int)ImageViewport.Width, (int)ImageViewport.Height);
			//_warpDevice = new WarpDevice((int) ImageViewport.Width, (int) ImageViewport.Height);
			_imageRenderer = new SwapChain(outputImage, (int)ImageViewport.Width, (int)ImageViewport.Height, 1);
			//_imageRenderer = new ImageRenderer(_warpDevice, outputImage, (int)ImageViewport.Width, (int)ImageViewport.Height, 1);

			//_warpDevice.SetRenderTarget(_imageRenderer.GetBuffer());

			ImageViewport.Source = outputImage;

			CreateRenderPipeline();
			CreateVertices();
			RefreshTriangle();

			//CompositionTarget.Rendering += CompositionTarget_Rendering;
		}

		void CompositionTarget_Rendering(object sender, System.EventArgs e)
		{
			_angle += 0.01f;
			RefreshTriangle();
		}

		private void CreateRenderPipeline()
		{
			

			//_device.RenderPipeline.InputAssembler.InputLayout = VertexPositionNormalTexture.InputLayout;
			//_device.RenderPipeline.InputAssembler.InputLayout = VertexPositionColor.InputLayout;
			//_device.RenderPipeline.InputAssembler.Vertices = _vertices;

			//_device.RenderPipeline.Rasterizer.FillMode = PipelineStages.Rasterizer.FillMode.Wireframe;

			_device.RenderPipeline.OutputMerger.RenderTarget = new RenderTargetView(_imageRenderer.GetBuffer());
			//_device.RenderPipeline.OutputMerger.BlendState = BlendState.AlphaBlend;

			//_basicEffect = new BasicEffect(_device);
		}

		private AxisAlignedBoundingBox _bounds;
		private void CreateVertices()
		{
			Scene scene = MeshellatorLoader.ImportFromFile(@"Assets\85-nissan-fairlady.3ds");
			//Scene scene = MeshellatorLoader.ImportFromFile(@"Assets\jeep1.3ds");
			_bounds = scene.Bounds;
			_model = ModelLoader.FromScene(_device, scene);

			/*List<ModelMesh> meshes = new List<ModelMesh>();
			List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>
			{
				// Triangle 1
				new VertexPositionNormalTexture(new Point3D(-150, 50, -50), Vector3D.Zero, new Point2D(0, 0)),
				new VertexPositionNormalTexture(new Point3D(100, 50, -30), Vector3D.Zero, new Point2D(1, 0)),
				new VertexPositionNormalTexture(new Point3D(-150, -50, -50), Vector3D.Zero, new Point2D(0, 1)),

				// Triangle 2
				new VertexPositionNormalTexture(new Point3D(100, 50, -30), Vector3D.Zero, new Point2D(1, 0)),
				new VertexPositionNormalTexture(new Point3D(-150, -50, -50), Vector3D.Zero, new Point2D(0, 1)),
				new VertexPositionNormalTexture(new Point3D(100, -50, -30), Vector3D.Zero, new Point2D(1, 1))
			};
			meshes.Add(new ModelMesh(_device)
			{
				Effect = new BasicEffect(_device, VertexPositionNormalTexture.InputLayout),
				Indices = new Nexus.Int32Collection(new[] { 0, 1, 2, 3, 4, 5 }),
				Vertices = vertices
			});
			_model = new Model(meshes);
			_bounds = new AxisAlignedBoundingBox(vertices.Select(v => v.Position));*/
		}

		private void RefreshTriangle()
		{
			//_device.ClearDepthBuffer(1);
			//_device.ClearRenderTarget(ColorsF.Green);
			DrawTriangle();
		}

		private void DrawTriangle()
		{
			Camera camera = PerspectiveCamera.CreateFromBounds(_bounds, MathUtility.PI_OVER_4);
			/*Camera camera = new PerspectiveCamera
			{
				FieldOfView = MathUtility.PI_OVER_4,
				FarPlaneDistance = 1000.0f,
				NearPlaneDistance = 1.0f,
				UpDirection = Vector3D.Up,
				LookDirection = Vector3D.Normalize(new Vector3D(-0.3f, 0.0f, -1.0f)),
				Position = new Point3D(100, 0, 400)
			};*/
			foreach (ModelMesh mesh in _model.Meshes)
			{
				BasicEffect effect = (BasicEffect) mesh.Effect;
				//DepthEffect effect = new DepthEffect(_device);
				//mesh.Effect = effect;
				effect.View = camera.GetViewMatrix();
				effect.Projection = camera.GetProjectionMatrix(_device.RenderPipeline.Rasterizer.Viewport.AspectRatio);
			}
			_model.Draw();

			/*_basicEffect.World = Matrix3D.CreateRotationY(_angle);
			_basicEffect.View = Matrix3D.CreateLookAt(Point3D.Zero, Vector3D.Forward, Vector3D.Up);
			_basicEffect.Projection = Matrix3D.CreatePerspectiveFieldOfView(MathUtility.PI_OVER_2 + MathUtility.PI_OVER_4,
				_renderTarget.Width / (float) _renderTarget.Height,
				1, 200);
			_basicEffect.Texture = _texture;
			//_basicEffect.VertexColorEnabled = true;
			_basicEffect.TextureColorEnabled = true;

			_basicEffect.CurrentTechnique.Passes[0].Apply();

			_device.Draw();*/

			//_warpDevice.EndScene();
			_imageRenderer.Present();
		}
	}
}