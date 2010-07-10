using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.InputAssembler;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.OutputMerger;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.ShaderStages.Core;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.Util;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.VertexAttributes;
using Nexus;
using Color = System.Windows.Media.Color;

namespace Apollo.Examples.SoftwareRasterizer.Views.Texturing
{
	public partial class Texturing : Page, IRenderTarget
	{
		private VertexInputTest[] _vertices = new VertexInputTest[3];
		private WriteableBitmapWrapper _renderTarget1, _renderTarget2;
		private RenderPipeline _renderPipeline;
		private BitmapImage _texture;
		private float _angle;

		public Texturing()
		{
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			_renderTarget1 = new WriteableBitmapWrapper((int) ImageViewport1.Width, (int) ImageViewport1.Height);
			ImageViewport1.Source = _renderTarget1.InnerBitmap;

			_renderTarget2 = new WriteableBitmapWrapper((int) ImageViewport2.Width, (int) ImageViewport2.Height);
			ImageViewport2.Source = _renderTarget2.InnerBitmap;

			CreateVertices();

			_texture = new BitmapImage(new Uri("Assets/TestTexture.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.None };
			_texture.ImageFailed += (sender2, e2) => { int i = 0; };
			_texture.ImageOpened += (sender2, e2) =>
			{
				CreateRenderPipeline();
				RefreshTriangle();
			};

			//CreateRenderPipeline();

			//RefreshTriangle();

			//CompositionTarget.Rendering += OnCompositionTargetRendering;
		}

		private void OnCompositionTargetRendering(object sender, System.EventArgs e)
		{
			//RefreshTriangle();
		}

		private void CreateRenderPipeline()
		{
			_renderPipeline = new RenderPipeline();

			_renderPipeline.InputAssembler.InputLayout = new InputLayout
			{
				Elements = new[]
				{
					new InputElementDescription("Position", VertexAttributeValueFormat.Point3D),
					new InputElementDescription("TexU", VertexAttributeValueFormat.Float),
					new InputElementDescription("TexV", VertexAttributeValueFormat.Float)
				}
			};
			_renderPipeline.InputAssembler.Vertices = _vertices;

			_renderPipeline.VertexShader.VertexShader = new VertexShaderInputTestShader
			{
				WorldViewProjection = Matrix3D.Identity // World
					* Matrix3D.CreateLookAt(Point3D.Zero, Vector3D.Forward, Vector3D.Up) // View
					* Matrix3D.CreatePerspectiveFieldOfView(MathUtility.PI_OVER_2 + MathUtility.PI_OVER_4, (float) (ImageViewport1.Width / ImageViewport1.Height), 1, 200) // Projection
			};

			_renderPipeline.PixelShader.PixelShader = new TexturedPixelShader
			{
				Texture = new Texture2D(new BitmapTextureImage2D(_texture))
			};

			_renderPipeline.OutputMerger.RenderTarget = this;
		}

		private void CreateVertices()
		{
			_vertices = new[]
			{
				// Triangle 1
				new VertexInputTest { Position = new Point3D(-150, 50, -50), TexU = 0, TexV = 0 },
				new VertexInputTest { Position = new Point3D(100, 50, -30), TexU = 1, TexV = 0 },
				new VertexInputTest { Position = new Point3D(-150, -50, -50), TexU = 0, TexV = 1 },

				// Triangle 2
				new VertexInputTest { Position = new Point3D(100, 50, -30), TexU = 1, TexV = 0 },
				new VertexInputTest { Position = new Point3D(-150, -50, -50), TexU = 0, TexV = 1 },
				new VertexInputTest { Position = new Point3D(100, -50, -30), TexU = 1, TexV = 1 }
			};
			/*_vertices = new[]
			{
				new ScreenVertex { Position = new Point3D(-1,  1, 0), TexU = 0, TexV = 0 },
				new ScreenVertex { Position = new Point3D( 1,  1, 0), TexU = 1, TexV = 0 },
				new ScreenVertex { Position = new Point3D(-1, -1, 0), TexU = 0, TexV = 1 },

				new ScreenVertex { Position = new Point3D( 1,  1, 0), TexU = 1, TexV = 0 },
				new ScreenVertex { Position = new Point3D(-1, -1, 0), TexU = 0, TexV = 1 },
				new ScreenVertex { Position = new Point3D( 1, -1, 0), TexU = 1, TexV = 1 }
			};*/
		}

		private void RefreshTriangle()
		{
			//_angle += 0.01f;
			/*_renderPipeline.VertexShader.VertexShader = new VertexShaderInputTestShader
			{
				WorldViewProjection = Matrix3D.CreateRotationY(_angle) // World
					* Matrix3D.CreateLookAt(Point3D.Zero, Vector3D.Forward, Vector3D.Up) // View
					* Matrix3D.CreatePerspectiveFieldOfView(MathUtility.PI_OVER_2 + MathUtility.PI_OVER_4, (float) (Width / Height), 1, 200) // Projection
			};*/

			_renderPipeline.Clear();
			DrawTriangle();
		}

		private void DrawTriangle()
		{
			_renderPipeline.Draw();
			_renderPipeline.OutputMerger.RenderTarget.EndFrame();
		}

		// http://msdn.microsoft.com/en-us/library/bb205123%28v=VS.85%29.aspx
		// http://msdn.microsoft.com/en-us/library/bb219690%28VS.85%29.aspx
		// http://msdn.microsoft.com/en-us/library/cc627092%28VS.85%29.aspx
		// http://msdn.microsoft.com/en-us/library/cc308049%28v=VS.85%29.aspx

		/*public struct ScreenVertex
		{
			public Point3D Position;
			public float TexU;
			public float TexV;
		}

		public struct ScreenVertexShaderOutput : IVertexShaderOutput
		{
			public Point4D Position { get; set; }
			public float TexU;
			public float TexV;
		}

		public class ScreenVertexShader : VertexShaderBase<ScreenVertex, ScreenVertexShaderOutput>
		{
			public override ScreenVertexShaderOutput Execute(ScreenVertex vertexShaderInput)
			{
				return new ScreenVertexShaderOutput
				{
					Position = new Point4D(vertexShaderInput.Position, 1),
					TexU = vertexShaderInput.TexU,
					TexV = vertexShaderInput.TexV
				};
			}
		}*/

		public struct VertexInputTest
		{
			public Point3D Position;
			public float TexU, TexV;
			//public ColorF Color;
		}

		public struct VertexShaderInputTest
		{
			public Point3D Position;
			public float TexU, TexV;
			//public ColorF Color;
		}

		public struct VertexShaderOutputTest : IVertexShaderOutput
		{
			public Point4D Position { get; set; }
			public float TexU, TexV;
			public float W;
			//public ColorF Color;
		}

		public class VertexShaderInputTestShader : VertexShaderBase<VertexShaderInputTest, VertexShaderOutputTest>
		{
			public Matrix3D WorldViewProjection { get; set; }

			public override VertexShaderOutputTest Execute(VertexShaderInputTest vertexShaderInput)
			{
				Point4D position = WorldViewProjection.Transform(vertexShaderInput.Position.ToHomogeneousPoint3D());
				return new VertexShaderOutputTest
				{
					Position = position,
					TexU = vertexShaderInput.TexU,
					TexV = vertexShaderInput.TexV,
					//Color = vertexShaderInput.Color
					W = position.W
				};
			}
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
			_renderTarget1.InnerBitmap.Invalidate();
		}
	}
}