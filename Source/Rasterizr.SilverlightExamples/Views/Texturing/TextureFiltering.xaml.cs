using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Nexus;
using Rasterizr.PipelineStages.OutputMerger;
using Rasterizr.PipelineStages.PerspectiveDivide;
using Rasterizr.PipelineStages.Rasterizer;
using Rasterizr.PipelineStages.ShaderStages.Core;
using Rasterizr.PipelineStages.ShaderStages.PixelShader;
using Rasterizr.PipelineStages.ShaderStages.VertexShader;
using Rasterizr.PipelineStages.TriangleSetup;
using Rasterizr.VertexAttributes;
using Color = System.Windows.Media.Color;
using Colors = System.Windows.Media.Colors;

namespace Rasterizr.SilverlightExamples.Views.Texturing
{
	public partial class TextureFiltering : Page, IRenderTarget
	{
		private Rectangle _draggedVertexMarker;
		private int _draggedVertexIndex;
		private readonly Rectangle[] _vertexMarkers = new Rectangle[4];
		private readonly Point[] _vertices = new Point[4];
		private PathFigure _rawTriangleFigure;
		private Texture2D _texture;
		private bool _loaded;

		public TextureFiltering()
		{
			InitializeComponent();

			Loaded += (sender, e) =>
			{
				_texture = new Texture2D("Assets/Koala.jpg");
				CreateVertices();
				RefreshTriangle();
				_loaded = true;
			};
		}

		private void CreateVertices()
		{
			_vertices[0] = new Point(50, 50);
			_vertices[1] = new Point(50, 200);
			_vertices[2] = new Point(200, 50);
			_vertices[3] = new Point(200, 200);

			for (int i = 0; i < _vertices.Length; ++i)
			{
				_vertexMarkers[i] = CreateVertexMarker(_vertices[i]);
				GridCanvas1.Children.Add(_vertexMarkers[i]);
			}

			_rawTriangleFigure = new PathFigure
			{
				IsClosed = true,
				StartPoint = _vertices[0],
				Segments = new PathSegmentCollection
				{
					new LineSegment { Point = _vertices[1] },
					new LineSegment { Point = _vertices[2] },
					new LineSegment { Point = _vertices[3] },
					new LineSegment { Point = _vertices[1] },
					new LineSegment { Point = _vertices[2] }
				}
			};
			GridCanvas1.Children.Add(new Path
			{
				IsHitTestVisible = false,
				Fill = new SolidColorBrush(Colors.Red) { Opacity = 0.2 },
				Stroke = new SolidColorBrush(Colors.Red),
				StrokeThickness = 1,
				Data = new PathGeometry
				{
					Figures = new PathFigureCollection
					{
						_rawTriangleFigure
					}
				}
			});
		}

		private Rectangle CreateVertexMarker(Point point)
		{
			Rectangle rectangle = new Rectangle
			{
				Width = 8,
				Height = 8,
				Fill = new SolidColorBrush(Colors.Red)
			};
			rectangle.SetValue(Canvas.LeftProperty, point.X - 4);
			rectangle.SetValue(Canvas.TopProperty, point.Y - 4);
			rectangle.MouseLeftButtonDown += OnVertexMarkerMouseLeftButtonDown;
			return rectangle;
		}

		private void OnVertexMarkerMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			_draggedVertexMarker = (Rectangle) e.OriginalSource;
			for (int i = 0; i < _vertexMarkers.Length; ++i)
				if (_draggedVertexMarker == _vertexMarkers[i])
				{
					_draggedVertexIndex = i;
					break;
				}
		}

		private void GridCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			_draggedVertexMarker = null;
			_draggedVertexIndex = -1;
		}

		private void GridCanvas_MouseMove(object sender, MouseEventArgs e)
		{
			if (_draggedVertexMarker != null)
			{
				Point newPoint = e.GetPosition(GridCanvas1);
				newPoint.X = Math.Min(Math.Max(newPoint.X, 0), GridCanvas1.Width);
				newPoint.Y = Math.Min(Math.Max(newPoint.Y, 0), GridCanvas1.Height);
				Canvas.SetLeft(_draggedVertexMarker, newPoint.X - 4);
				Canvas.SetTop(_draggedVertexMarker, newPoint.Y - 4);
				_vertices[_draggedVertexIndex] = newPoint;
				RefreshTriangle();
			}
		}

		private void RefreshTriangle()
		{
			_rawTriangleFigure.StartPoint = _vertices[0];
			((LineSegment) _rawTriangleFigure.Segments[0]).Point = _vertices[1];
			((LineSegment) _rawTriangleFigure.Segments[1]).Point = _vertices[2];
			((LineSegment) _rawTriangleFigure.Segments[2]).Point = _vertices[3];
			((LineSegment) _rawTriangleFigure.Segments[3]).Point = _vertices[1];
			((LineSegment) _rawTriangleFigure.Segments[4]).Point = _vertices[2];

			ScreenGrid1.Clear();

			DrawTriangle(_vertices[0], _vertices[1], _vertices[2], _vertices[3]);
		}

		private VertexShaderOutput CreateVertexShaderOutput(Viewport3D viewport, Matrix3D view, Matrix3D projection, Matrix3D wvp, Point point, float z, float u, float v, out Point4D actualPoint)
		{
			Point3D screenPoint = new Point3D(ScreenGrid1.TransformToScreen(point.X), ScreenGrid1.TransformToScreen(point.Y), wvp.Transform(new Point3D(0, 0, z)).Z);
			Point3D scenePoint = viewport.Unproject(screenPoint, projection, view, Matrix3D.Identity);
			actualPoint = new Point4D(scenePoint.X, scenePoint.Y, z, 1);
			Point4D transformedPoint = wvp.Transform(actualPoint);

			return new VertexShaderOutput
			{
				Position = transformedPoint,
				Attributes = new[]
				{
					new VertexAttribute
					{
						Name = "TexCoords",
						InterpolationType = VertexAttributeInterpolationType.Perspective,
						Value = new Point2DVertexAttributeValue
						{
							Value = new Point2D(u, v)
						}
					}
				}
			};
		}

		private void DrawTriangle(Point v1, Point v2, Point v3, Point v4)
		{
			Viewport3D viewport = new Viewport3D
			{
				Width = ScreenGrid1.NumColumns,
				Height = ScreenGrid1.NumRows,
				MinDepth = 0,
				MaxDepth = 1
			};
			Matrix3D view = Matrix3D.CreateLookAt(Point3D.Zero, Vector3D.Forward, Vector3D.Up);
			Matrix3D projection = Matrix3D.CreatePerspectiveFieldOfView(MathUtility.PI_OVER_2 + MathUtility.PI_OVER_4, (ScreenGrid1.NumColumns / (float) ScreenGrid1.NumRows), 1, 200);
			Matrix3D wvp = Matrix3D.Identity * view * projection;
			Point4D[] actualPoints = new Point4D[3];
			VertexShaderOutput[] vertexShaderOutputs = new[]
			{
				CreateVertexShaderOutput(viewport, view, projection, wvp, v1, -30, 0, 0, out actualPoints[0]),
				CreateVertexShaderOutput(viewport, view, projection, wvp, v2, -30, 0, 1, out actualPoints[1]),
				CreateVertexShaderOutput(viewport, view, projection, wvp, v3, -30, 1, 0, out actualPoints[2]),

				CreateVertexShaderOutput(viewport, view, projection, wvp, v2, -30, 0, 1, out actualPoints[1]),
				CreateVertexShaderOutput(viewport, view, projection, wvp, v3, -30, 1, 0, out actualPoints[2]),
				CreateVertexShaderOutput(viewport, view, projection, wvp, v4, -30, 1, 1, out actualPoints[2])
			};

			PerspectiveDivideStage perspectiveDivideStage = new PerspectiveDivideStage(new Viewport3D())
			{
				ScreenWidth = ScreenGrid1.NumColumns,
				ScreenHeight = ScreenGrid1.NumRows
			};

			List<ScreenSpaceVertex> screenSpaceVertices = new List<ScreenSpaceVertex>();
			perspectiveDivideStage.Process(vertexShaderOutputs, screenSpaceVertices);

			VertexLocations.Text = string.Format("V1 = {1}, {2}, {3}{0}V2 = {4}, {5}, {6}{0}V3 = {7}, {8}, {9}",
				Environment.NewLine,
				actualPoints[0].X, actualPoints[0].Y, actualPoints[0].Z,
				actualPoints[1].X, actualPoints[1].Y, actualPoints[1].Z,
				actualPoints[2].X, actualPoints[2].Y, actualPoints[2].Z);

			// Triangle setup
			TriangleSetupStage triangleSetupStage = new TriangleSetupStage();
			List<Triangle> triangles = new List<Triangle>();
			triangleSetupStage.Process(screenSpaceVertices, triangles);

			RasterizerStage rasterizerStage = new RasterizerStage(new Viewport3D());
			List<Fragment> fragments = new List<Fragment>();
			rasterizerStage.Process(triangles, fragments);

			TextureFilter magFilter = (TextureFilter) Enum.Parse(typeof(TextureFilter), (string) ((ComboBoxItem) cboMagFilter.SelectedValue).Content, true);
			TextureFilter minFilter = (TextureFilter) Enum.Parse(typeof(TextureFilter), (string) ((ComboBoxItem) cboMinFilter.SelectedValue).Content, true);
			TextureMipMapFilter mipFilter = (TextureMipMapFilter) Enum.Parse(typeof(TextureMipMapFilter), (string) ((ComboBoxItem) cboMipFilter.SelectedValue).Content, true);
			PixelShaderStage pixelShaderStage = new PixelShaderStage(new Viewport3D())
			{
				PixelShader = new TexturedPixelShader
				{
					Texture = _texture,
					Sampler =
						{
							MagFilter = magFilter,
							MinFilter = minFilter,
							MipFilter = mipFilter
						}
				}
			};
			List<Pixel> pixels = new List<Pixel>();
			pixelShaderStage.Process(fragments, pixels);

			OutputMergerStage outputMergerStage = new OutputMergerStage
			{
				RenderTarget = this
			};
			outputMergerStage.Process(pixels);
		}

		#region IRenderTarget implementation

		public void Clear()
		{
			ScreenGrid1.Clear();
		}

		public Nexus.Color GetPixel(int x, int y)
		{
			/*Color color = ScreenGrid1.GetPixel(x, y);
			return new Color(color.A, color.R, color.G, color.B);*/
			throw new NotImplementedException();
		}

		public void SetPixel(int x, int y, Nexus.Color color)
		{
			ScreenGrid1.SetPixel(x, y, new SolidColorBrush(Color.FromArgb(
				color.A, color.R, color.G, color.B)));
		}

		public void BeginFrame()
		{

		}

		public void EndFrame()
		{
			//ScreenGrid1._renderTarget.InnerBitmap.Invalidate();
		}

		#endregion

		private void cboMagFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!_loaded)
				return;

			RefreshTriangle();
		}

		private void cboMipFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!_loaded)
				return;

			RefreshTriangle();
		}

		private void cboMinFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!_loaded)
				return;

			RefreshTriangle();
		}
	}
}