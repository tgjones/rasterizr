using System.Windows.Controls;
using Caliburn.Micro;

namespace Rasterizr.Studio.Modules.SampleBrowser.Samples
{
	/// <summary>
	/// Interaction logic for SampleView.xaml
	/// </summary>
	public partial class SampleView : UserControl
	{
		public SampleView()
		{
			InitializeComponent();

			IsVisibleChanged += (sender, e) =>
			{
				if (IsVisible)
					((IActivate) DataContext).Activate();
				else
					((IDeactivate) DataContext).Deactivate(false);
			};
		}
	}
}
