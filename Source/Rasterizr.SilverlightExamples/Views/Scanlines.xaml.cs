using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Apollo.Graphics.Rendering;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.PerspectiveDivide;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.Rasterizer;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.TriangleSetup;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.VertexShader.VertexAttributes;
using Nexus;
using Color = Nexus.Color;
using Colors = System.Windows.Media.Colors;

namespace Apollo.Examples.SoftwareRasterizer.Views
{
	public partial class Scanlines : Page
	{
		private static readonly SolidColorBrush BlueBrush = new SolidColorBrush(Colors.Blue);
		private static readonly SolidColorBrush GreenBrush = new SolidColorBrush(Colors.Green);

		private Rectangle _draggedVertexMarker;
		private int _draggedVertexIndex;
		private readonly Rectangle[] _vertexMarkers = new Rectangle[3];
		private readonly Point[] _vertices = new Point[3];
		private PathFigure _rawTriangleFigure;

		public Scanlines()
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
				GridCanvas1.Children.Add(_vertexMarkers[i]);
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

			ScreenGrid1.Clear();
			ScreenGrid2.Clear();
			ScreenGrid3.Clear();

			DrawTriangle(_vertices[0], _vertices[1], _vertices[2]);
		}

		private VertexShaderOutput CreateVertexShaderOutput(Viewport3D viewport, Matrix3D view, Matrix3D projection, Matrix3D wvp, Point point, float z, ColorF color, out Point4D actualPoint)
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
						Name = "Color",
						InterpolationType = VertexAttributeInterpolationType.Perspective,
						Value = new ColorFVertexAttributeValue
						{
							Value = color
						}
					}
				}
			};
		}

		private void DrawTriangle(Point v1, Point v2, Point v3)
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
				CreateVertexShaderOutput(viewport, view, projection, wvp, v1, -30, ColorsF.White, out actualPoints[0]),
				CreateVertexShaderOutput(viewport, view, projection, wvp, v2, -30, ColorsF.Red, out actualPoints[1]),
				CreateVertexShaderOutput(viewport, view, projection, wvp, v3, -20, ColorsF.Red, out actualPoints[2])
			};

			PerspectiveDivideStage perspectiveDivideStage = new PerspectiveDivideStage
			{
				ScreenWidth = ScreenGrid1.NumColumns,
				ScreenHeight = ScreenGrid1.NumRows
			};

			List<ScreenSpaceVertex> screenSpaceVertices = new List<ScreenSpaceVertex>();
			perspectiveDivideStage.Process(vertexShaderOutputs, screenSpaceVertices);

			/*for (int i = 0; i < screenSpaceVertices.Count; ++i)
				screenSpaceVertices[i] = new ScreenSpaceVertex
				{
					Position = new Point3D(ScreenGrid.TransformToScreen(screenSpaceVertices[i].Position.X), ScreenGrid.TransformToScreen(screenSpaceVertices[i].Position.Y), screenSpaceVertices[i].Position.Z),
					Attributes = screenSpaceVertices[i].Attributes,
					W = screenSpaceVertices[i].W
				};*/

			VertexLocations.Text = string.Format("V1 = {1}, {2}, {3}{0}V2 = {4}, {5}, {6}{0}V3 = {7}, {8}, {9}",
				Environment.NewLine,
				actualPoints[0].X, actualPoints[0].Y, actualPoints[0].Z,
				actualPoints[1].X, actualPoints[1].Y, actualPoints[1].Z,
				actualPoints[2].X, actualPoints[2].Y, actualPoints[2].Z);

			// Triangle setup

			TriangleSetupStage triangleSetupStage = new TriangleSetupStage();

			Triangle triangle;
			TriangleSetupInfo triangleSetupInfo;
			triangleSetupStage.BuildTriangle(screenSpaceVertices.ToArray(), out triangle, out triangleSetupInfo);

			// Scanlines
			for (int i = 0; i < triangle.Scanlines.Length; ++i)
				DrawScanline(triangle, triangle.Scanlines[i]);

			// Gradients and vertices
			TriangleGradients.Text = triangleSetupInfo.Gradients.ToString();
			for (int i = 0; i < triangleSetupInfo.Gradients.InterpolatedVertexAttributes.Length; ++i)
			{
				ColorF color = (ColorF) triangleSetupInfo.Gradients.InterpolatedVertexAttributes[i].Attributes[0].Value.Multiply(1 / triangleSetupInfo.Gradients.InterpolatedVertexAttributes[i].OneOverW).Value;
				ScreenGrid2.SetPixel((int) screenSpaceVertices[i].Position.X,
					(int) screenSpaceVertices[i].Position.Y,
					GetBrush(color));
			}

			// Edges.
			DrawEdge(triangleSetupInfo.TopToBottomEdge);
		}

		private void DrawEdge(Edge edge)
		{
			
			//ScreenGrid3.SetPixel();
		}

		private void DrawScanline(Triangle triangle, Scanline scanline)
		{
			string scanlineTooltip = "SCANLINE DETAILS" + Environment.NewLine
				+ "X Prestep = " + scanline.XPrestep + Environment.NewLine
				+ "OneOverW = " + scanline.InterpolatedVertexAttributes.OneOverW + Environment.NewLine + Environment.NewLine
				+ "INTERPOLATED VERTEX ATTRIBUTES" + Environment.NewLine;
			foreach (var vertexAttribute in scanline.InterpolatedVertexAttributes.Attributes)
			{
				scanlineTooltip += vertexAttribute.Name + " = " + vertexAttribute.Value.Value + Environment.NewLine;
			}
			scanlineTooltip += Environment.NewLine;

			RasterizerStage rasterizerStage = new RasterizerStage();
			List<Fragment> fragments = new List<Fragment>();
			rasterizerStage.ProcessScanline(triangle, scanline, fragments);
			//ScreenGrid.SetPixel(scanline.XStart, scanline.Y, BlueBrush, scanlineToolip);
			foreach (Fragment fragment in fragments)
			{
				string tooltip = fragment.ToString();
				ColorF fragmentColor = (ColorF) fragment.Attributes[0].Value.Value;
				ScreenGrid1.SetPixel(fragment.X, fragment.Y, GetBrush(fragmentColor), tooltip);
			}
			//ScreenGrid.SetPixel(scanline.XStart + scanline.Width, scanline.Y, BlueBrush, scanlineToolip);
		}

		private static SolidColorBrush GetBrush(ColorF color)
		{
			Color fragmentColor = color.ToRgbColor();
			return new SolidColorBrush(System.Windows.Media.Color.FromArgb(fragmentColor.A, fragmentColor.R, fragmentColor.G, fragmentColor.B));
		}
	}
}