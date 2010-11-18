using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Nexus;
using Rasterizr.PipelineStages.OutputMerger;
using Rasterizr.PipelineStages.ShaderStages.Core;
using Rasterizr.Util;
using Color = System.Windows.Media.Color;

namespace Rasterizr.SilverlightExamples.Views.Texturing
{
	public partial class Texturing : Page
	{
		/*private VertexPositionColor[] _vertices = new VertexPositionColor[3];
		private RasterizrDevice _device;
		private BasicEffect _basicEffect;

		private WriteableBitmapWrapper _renderTarget1, _renderTarget2;
		private Texture2D _texture;

		private float _angle;

		public Texturing()
		{
			InitializeComponent();
		}

		*/private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			/*WriteableBitmap renderTarget1 = new WriteableBitmap((int)ImageViewport1.Width, (int)ImageViewport1.Height);
			_renderTarget1 = new WriteableBitmapWrapper(renderTarget1);
			ImageViewport1.Source = renderTarget1;

			WriteableBitmap renderTarget2 = new WriteableBitmap((int)ImageViewport2.Width, (int)ImageViewport2.Height);
			_renderTarget2 = new WriteableBitmapWrapper(renderTarget2);
			ImageViewport2.Source = renderTarget2;

			CreateVertices();

			_texture = new Texture2D("Rasterizr.SilverlightExamples;component/Assets/TestTexture.png");

			CreateRenderPipeline();
			RefreshTriangle();

			CompositionTarget.Rendering += CompositionTarget_Rendering;*/
		}

		/*void CompositionTarget_Rendering(object sender, System.EventArgs e)
		{
			//_angle += 0.01f;
			//RefreshTriangle();
		}

		private void CreateRenderPipeline()
		{
			_device = new RasterizrDevice((int)ImageViewport1.Width, (int)ImageViewport1.Height);

			//_device.RenderPipeline.InputAssembler.InputLayout = VertexPositionNormalTexture.InputLayout;
			_device.RenderPipeline.InputAssembler.InputLayout = VertexPositionColor.InputLayout;
			_device.RenderPipeline.InputAssembler.Vertices = _vertices;

			_device.RenderPipeline.OutputMerger.RenderTarget = this;

			_basicEffect = new BasicEffect(_device, VertexPositionColor.InputLayout);
		}

		private void CreateVertices()
		{
			_vertices = new[]
			{
				// Triangle 1
				//new VertexPositionNormalTexture(new Point3D(-150, 50, -50), Vector3D.Zero, new Point2D(0, 0)),
				//new VertexPositionNormalTexture(new Point3D(100, 50, -30), Vector3D.Zero, new Point2D(1, 0)),
				//new VertexPositionNormalTexture(new Point3D(-150, -50, -50), Vector3D.Zero, new Point2D(0, 1)),

				// Triangle 2
				//new VertexPositionNormalTexture(new Point3D(100, 50, -30), Vector3D.Zero, new Point2D(1, 0)),
				//new VertexPositionNormalTexture(new Point3D(-150, -50, -50), Vector3D.Zero, new Point2D(0, 1)),
				//new VertexPositionNormalTexture(new Point3D(100, -50, -30), Vector3D.Zero, new Point2D(1, 1))
				// Triangle 1
				new VertexPositionColor(new Point3D(-150, 50, -50), ColorsF.Red),
				new VertexPositionColor(new Point3D(100, 50, -30), ColorsF.Blue),
				new VertexPositionColor(new Point3D(-150, -50, -50), ColorsF.Green),

				// Triangle 2
				new VertexPositionColor(new Point3D(100, 50, -30), ColorsF.Blue),
				new VertexPositionColor(new Point3D(-150, -50, -50), ColorsF.Green),
				new VertexPositionColor(new Point3D(-150, -50, -50), ColorsF.White)
			};
		}

		private void RefreshTriangle()
		{
			_device.RenderPipeline.Clear();
			DrawTriangle();
		}

		private void DrawTriangle()
		{
			_basicEffect.World = Matrix3D.CreateRotationY(_angle);
			_basicEffect.View = Matrix3D.CreateLookAt(Point3D.Zero, Vector3D.Forward, Vector3D.Up);
			_basicEffect.Projection = Matrix3D.CreatePerspectiveFieldOfView(MathUtility.PI_OVER_2 + MathUtility.PI_OVER_4,
				_renderTarget1.Width / (float) _renderTarget1.Height,
				1, 200);
			_basicEffect.Texture = _texture;
			_basicEffect.VertexColorEnabled = true;

			_basicEffect.CurrentTechnique.Passes[0].Apply();

			_device.Draw();

			_device.RenderPipeline.OutputMerger.RenderTarget.EndFrame();
		}

		int IRenderTarget.Width
		{
			get { return _renderTarget1.Width; }
		}

		int IRenderTarget.Height
		{
			get { return _renderTarget1.Height; }
		}

		public void Clear()
		{
			_renderTarget1.Clear(System.Windows.Media.Colors.LightGray);
			_renderTarget2.Clear(System.Windows.Media.Colors.LightGray);
		}

		public Nexus.Color GetPixel(int x, int y)
		{
			Color color = _renderTarget1.GetPixel(x, y);
			return new Nexus.Color(color.A, color.R, color.G, color.B);
		}

		public void SetPixel(int x, int y, Nexus.Color color)
		{
			_renderTarget1.SetPixel(x, y, Color.FromArgb(
				color.A, color.R, color.G, color.B));
		}

		public void BeginFrame()
		{
			
		}

		public void EndFrame()
		{
			_renderTarget1.Invalidate();
		}*/
	}
}