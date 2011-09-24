using System.Windows;
using Rasterizr.Studio.Framework;

namespace Rasterizr.Studio
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			var mainWindowView = new MainWindowView();
			mainWindowView.DataContext = new MainWindowViewModel();
			mainWindowView.Show();

			base.OnStartup(e);
		}
	}
}
