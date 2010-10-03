using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Nexus;
using Rasterizr.PipelineStages.PerspectiveDivide;
using Rasterizr.PipelineStages.Rasterizer;
using Rasterizr.PipelineStages.ShaderStages.PixelShader;
using Rasterizr.PipelineStages.TriangleSetup;
using Rasterizr.VertexAttributes;
using Color = Nexus.Color;
using Colors = System.Windows.Media.Colors;

namespace Rasterizr.SilverlightExamples.Views
{
	public partial class TriangleRasterization : Page
	{
		private static readonly SolidColorBrush BlueBrush = new SolidColorBrush(Colors.Blue);
		private static readonly SolidColorBrush GreenBrush = new SolidColorBrush(Colors.Green);

		private Rectangle _draggedVertexMarker;
		private int _draggedVertexIndex;
		private readonly Rectangle[] _vertexMarkers = new Rectangle[3];
		private readonly Point[] _vertices = new Point[3];
		private PathFigure _rawTriangleFigure;

		public TriangleRasterization()
		{
			InitializeComponent();

			Loaded += (sender, e) =>
			{
				CreateVertices();
				RefreshTriangle();
			};
		}

		private void CreateVertices()
		{
			_vertices[0] = new Point(50, 50);
			_vertices[1] = new Point(50, 200);
			_vertices[2] = new Point(200, 150);

			for (int i = 0; i < _vertices.Length; ++i)
			{
				_vertexMarkers[i] = CreateVertexMarker(_vertices[i]);
				GridCanvas.Children.Add(_vertexMarkers[i]);
			}

			_rawTriangleFigure = new PathFigure
			{
				IsClosed = true,
				StartPoint = _vertices[0],
				Segments = new PathSegmentCollection
				{
					new LineSegment { Point = _vertices[1] },
					new LineSegment { Point = _vertices[2] }
				}
			};
			GridCanvas.Children.Add(new Path
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
				Point newPoint = e.GetPosition(GridCanvas);
				newPoint.X = Math.Min(Math.Max(newPoint.X, 0), GridCanvas.Width);
				newPoint.Y = Math.Min(Math.Max(newPoint.Y, 0), GridCanvas.Height);
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

			ScreenGrid.Clear();

			DrawTriangle(_vertices[0], _vertices[1], _vertices[2]);
		}

		private Point3D TransformToScreen(Point point)
		{
			return new Point3D(ScreenGrid.TransformToScreen(point.X), ScreenGrid.TransformToScreen(point.Y), 100);
		}

		private void DrawTriangle(Point v1, Point v2, Point v3)
		{
			ScreenSpaceVertex[] vertices = new[]
			{
				new ScreenSpaceVertex { Position = TransformToScreen(v1), W = 1.0f, Attributes = new VertexAttributeCollection { new VertexAttribute { Name = "Color", InterpolationType = VertexAttributeInterpolationType.Perspective, Value = new ColorFVertexAttributeValue { Value = ColorsF.Red } } } },
				new ScreenSpaceVertex { Position = TransformToScreen(v2), W = 1.0f, Attributes = new VertexAttributeCollection { new VertexAttribute { Name = "Color", InterpolationType = VertexAttributeInterpolationType.Perspective, Value = new ColorFVertexAttributeValue { Value = ColorsF.Blue } } } },
				new ScreenSpaceVertex { Position = TransformToScreen(v3), W = 1.0f, Attributes = new VertexAttributeCollection { new VertexAttribute { Name = "Color", InterpolationType = VertexAttributeInterpolationType.Perspective, Value = new ColorFVertexAttributeValue { Value = ColorsF.Green } } } }
			};

			VertexLocations.Text = string.Format("V1 = {1}, {2}{0}V2 = {3}, {4}{0}V3 = {5}, {6}",
				Environment.NewLine,
				vertices[0].Position.X, vertices[0].Position.Y,
				vertices[1].Position.X, vertices[1].Position.Y,
				vertices[2].Position.X, vertices[2].Position.Y);

			// Triangle setup

			TriangleSetupStage triangleSetupStage = new TriangleSetupStage();

			IList<Triangle> triangles = new List<Triangle>();
			triangleSetupStage.Process(vertices, triangles);

			/*foreach (Triangle triangle in triangles)
				for (int i = 0; i < triangle.Scanlines.Length; ++i)
					DrawScanline(triangle.Scanlines[i]);*/

			// Rasterizer

			RasterizerStage rasterizerStage = new RasterizerStage(new Viewport3D());

			IList<Fragment> fragments = new List<Fragment>();
			rasterizerStage.Process(triangles, fragments);

			// Pixel shader

			PixelShaderStage pixelShaderStage = new PixelShaderStage(new Viewport3D());
			pixelShaderStage.PixelShader = new PixelShaderInputTestShader();

			IList<Pixel> pixels = new List<Pixel>();
			pixelShaderStage.Process(fragments, pixels);

			foreach (Pixel pixel in pixels)
			{
				Color color = (Color) pixel.Color;
				ScreenGrid.SetPixel(pixel.X, pixel.Y,
														new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R,
																																										color.G, color.B)));
			}
		}

		private void DrawScanline(Scanline scanline)
		{
			ScreenGrid.SetPixel(scanline.XStart, scanline.Y, BlueBrush);
			for (int x = scanline.XStart + 1; x < scanline.XStart + scanline.Width; ++x)
				ScreenGrid.SetPixel(x, scanline.Y, GreenBrush);
			ScreenGrid.SetPixel(scanline.XStart + scanline.Width, scanline.Y, BlueBrush);
		}

		// http://msdn.microsoft.com/en-us/library/bb205123%28v=VS.85%29.aspx
		// http://msdn.microsoft.com/en-us/library/bb219690%28VS.85%29.aspx
		// http://msdn.microsoft.com/en-us/library/cc627092%28VS.85%29.aspx
		// http://msdn.microsoft.com/en-us/library/cc308049%28v=VS.85%29.aspx

		/*public struct VertexShaderOutputTest : IVertexShaderOutput
		{
			public Point4D Position { get; set; }
			public ColorF Color;
			public float U; // Perspective
			public float V; // Perspective
			public float T; // Linear
			public int I; // Constant
		}*/

		public class PixelShaderInputTestShader : PixelShaderBase<PixelShaderInputTest>
		{
			public override ColorF Execute(PixelShaderInputTest pixelShaderInput)
			{
				//return ColorF.Saturate(pixelShaderInput.Color).ToRgbColor();
				return pixelShaderInput.Color;
			}
		}

		public struct PixelShaderInputTest
		{
			public ColorF Color;
			//public float U;
			//public float V;
		}
	}
}