using System.Windows.Controls;
using System.Windows.Input;
using Rasterizr.Studio.Framework;
using Rasterizr.Studio.Modules.TracefileViewer.ViewModels;

namespace Rasterizr.Studio.Modules.TracefileViewer.Views
{
	/// <summary>
	/// Interaction logic for TracefileView.xaml
	/// </summary>
	public partial class TracefileViewerView : UserControl
	{
		public TracefileViewerView()
		{
			InitializeComponent();
		}

		private void OnFrameImageMouseMove(object sender, MouseEventArgs e)
		{
			var vm = (TracefileViewerViewModel) DataContext;
			var position = e.GetPosition(FrameImage);
			vm.HoverPixel = new Int32Point((int) position.X, (int) position.Y);
		}

		private void OnFrameImageMouseDown(object sender, MouseButtonEventArgs e)
		{
			var vm = (TracefileViewerViewModel)DataContext;
			var position = e.GetPosition(FrameImage);
			vm.SelectedPixel = new Int32Point((int)position.X, (int)position.Y);
		}

		private void OnFrameImageMouseLeave(object sender, MouseEventArgs e)
		{
			var vm = (TracefileViewerViewModel)DataContext;
			vm.HasHoverPixel = false;
		}
	}
}
