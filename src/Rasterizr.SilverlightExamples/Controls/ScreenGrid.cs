using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Rasterizr.SilverlightExamples.Controls
{
	public class ScreenGrid : Panel
	{
		private static readonly SolidColorBrush WhiteBrush = new SolidColorBrush(Colors.White);
		private static readonly SolidColorBrush LightGrayBrush = new SolidColorBrush(Colors.LightGray);
		private static readonly SolidColorBrush DarkGrayBrush = new SolidColorBrush(Colors.DarkGray);
		private static readonly SolidColorBrush BlackBrush = new SolidColorBrush(Colors.Black);

		public int CellSize { get; set; }

		private bool _gridCreated;
		private Grid _grid;
		private Border[,] _squares;
		private readonly List<Border> _drawnPixels = new List<Border>();

		public ScreenGrid()
		{
			CellSize = 20;
			Loaded += (sender, e) => EnsureGrid();
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			EnsureGrid();
			_grid.Measure(availableSize);
			return _grid.DesiredSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			_grid.Arrange(new Rect(new Point(), finalSize));
			return new Size(_grid.ActualWidth, _grid.ActualHeight);
		}

		private void EnsureGrid()
		{
			if (!_gridCreated)
			{
				CreateGrid();
				_gridCreated = true;
			}
		}

		public int NumColumns
		{
			get { return (int) (Width / CellSize); }
		}

		public int NumRows
		{
			get { return (int) (Height / CellSize); }
		}

		private void CreateGrid()
		{
			_grid = new Grid { Width = Width, Height = Height };
			Children.Add(_grid);

			int numColumns = NumColumns;
			int numRows = NumRows;
			_squares = new Border[numColumns, numRows];

			for (int i = 0; i < numRows; ++i)
			{
				RowDefinition rd = new RowDefinition();
				_grid.RowDefinitions.Add(rd);
			}

			for (int i = 0; i < numColumns; ++i)
			{
				ColumnDefinition cd = new ColumnDefinition();
				_grid.ColumnDefinitions.Add(cd);
			}

			for (int i = 0; i < numRows; ++i)
				for (int j = 0; j < numColumns; ++j)
					AddSquare(i, j, numRows, numColumns);
		}

		private void AddSquare(int row, int column, int numRows, int numColumns)
		{
			Thickness thickness = new Thickness(1, 1, 0, 0);
			if (row == numRows - 1)
				thickness.Bottom = 1;
			if (column == numColumns - 1)
				thickness.Right = 1;

			Border border = new Border
			{
				Background = LightGrayBrush,
				BorderBrush = DarkGrayBrush,
				BorderThickness = thickness
			};
			_grid.Children.Add(border);
			border.SetValue(Grid.RowProperty, row);
			border.SetValue(Grid.ColumnProperty, column);

			border.Child = new Rectangle
			{
				Fill = BlackBrush,
				Width = 2,
				Height = 2
			};

			_squares[column, row] = border;
		}

		public void Clear()
		{
			_drawnPixels.ForEach(b =>
			{
				b.Background = LightGrayBrush;
				ToolTipService.SetToolTip(b, null);
			});
			_drawnPixels.Clear();
		}

		public void SetPixel(int x, int y, Brush brush, string tooltip = null)
		{
			if (x < 0 || y < 0 || x > NumColumns - 1 || y > NumRows - 1)
				return;

			_drawnPixels.Add(_squares[x, y]);
			_squares[x, y].Background = brush;

			if (tooltip != null)
				ToolTipService.SetToolTip(_squares[x, y], new ToolTip
				{
					Content = tooltip
				});
		}

		public float TransformToScreen(double value)
		{
			return (float) decimal.Round(((decimal) (value / CellSize)), 1);
		}
	}
}
