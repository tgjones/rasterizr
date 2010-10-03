using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Meshellator;
using Meshellator.Integration.Rasterizr;
using Nexus;
using Nexus.Graphics.Cameras;
using Rasterizr.PipelineStages.OutputMerger;
using Rasterizr.PipelineStages.ShaderStages.Core;
using Rasterizr.Util;
using Color = System.Windows.Media.Color;

namespace Rasterizr.WpfExamples
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Model _model;
		private RasterizrDevice _device;
		//private BasicEffect _basicEffect;

		private WriteableBitmapRenderTarget _renderTarget;
		private Texture2D _texture;

		private float _angle;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			WriteableBitmap renderTarget = new WriteableBitmap((int) ImageViewport.Width, (int) ImageViewport.Height,
				96, 96, PixelFormats.Bgra32, null);
			_renderTarget = new WriteableBitmapRenderTarget(renderTarget);
			ImageViewport.Source = renderTarget;

			

			_texture = new Texture2D("pack://application:,,,/Rasterizr.WpfExamples;component/Assets/TestTexture.png");

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
			_device = new RasterizrDevice((int) ImageViewport.Width, (int) ImageViewport.Height);

			//_device.RenderPipeline.InputAssembler.InputLayout = VertexPositionNormalTexture.InputLayout;
			//_device.RenderPipeline.InputAssembler.InputLayout = VertexPositionColor.InputLayout;
			//_device.RenderPipeline.InputAssembler.Vertices = _vertices;

			_device.RenderPipeline.OutputMerger.RenderTarget = _renderTarget;
			_device.RenderPipeline.OutputMerger.BlendState = BlendState.AlphaBlend;

			//_basicEffect = new BasicEffect(_device);
		}

		private AxisAlignedBoundingBox _bounds;
		private void CreateVertices()
		{
			Scene scene = MeshellatorLoader.ImportFromFile(@"Assets\85-nissan-fairlady.3ds");
			_bounds = scene.Bounds;
			_model = ModelLoader.FromScene(_device, scene);
			/*_vertices = new[]
			{
				// Triangle 1
				new VertexPositionNormalTexture(new Point3D(-150, 50, -50), Vector3D.Zero, new Point2D(0, 0)),
				new VertexPositionNormalTexture(new Point3D(100, 50, -30), Vector3D.Zero, new Point2D(1, 0)),
				new VertexPositionNormalTexture(new Point3D(-150, -50, -50), Vector3D.Zero, new Point2D(0, 1)),

				// Triangle 2
				new VertexPositionNormalTexture(new Point3D(100, 50, -30), Vector3D.Zero, new Point2D(1, 0)),
				new VertexPositionNormalTexture(new Point3D(-150, -50, -50), Vector3D.Zero, new Point2D(0, 1)),
				new VertexPositionNormalTexture(new Point3D(100, -50, -30), Vector3D.Zero, new Point2D(1, 1))*/
				/*// Triangle 1
				new VertexPositionColor(new Point3D(-150, 50, -50), ColorsF.Red),
				new VertexPositionColor(new Point3D(100, 50, -30), ColorsF.Blue),
				new VertexPositionColor(new Point3D(-150, -50, -50), ColorsF.Green),

				// Triangle 2
				new VertexPositionColor(new Point3D(100, 50, -30), ColorsF.Blue),
				new VertexPositionColor(new Point3D(-150, -50, -50), ColorsF.Green),
				new VertexPositionColor(new Point3D(-150, -50, -50), ColorsF.White)*/
			//};
		}

		private void RefreshTriangle()
		{
			_device.RenderPipeline.Clear();
			DrawTriangle();
		}

		private void DrawTriangle()
		{
			Camera camera = PerspectiveCamera.CreateFromBounds(_bounds, MathUtility.PI_OVER_4);
			foreach (ModelMesh mesh in _model.Meshes)
			{
				BasicEffect effect = (BasicEffect) mesh.Effect;
				effect.View = camera.GetViewMatrix();
				effect.Projection = camera.GetProjectionMatrix(_renderTarget.Width / (float) _renderTarget.Height);
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

			_device.Present();
		}
	}
}
