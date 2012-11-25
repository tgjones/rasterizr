using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Rasterizr.Samples.Common
{
	public abstract class WpfDemoApp : DemoApp
	{
		private Window _window;
		private Image _image;

		protected Image Image
		{
			get { return _image; }
		}

		protected override void CreateWindow()
		{
			_window = new Window
			{
				SizeToContent = SizeToContent.WidthAndHeight
			};
			_image = new Image
			{
				Width = Config.Width,
				Height = Config.Height
			};
			_window.Content = _image;
		}

		protected override void DoLoop()
		{
			bool isWindowClosed = false;

			_window.Closed += (sender, e) => isWindowClosed = true;

			var timer = new DispatcherTimer();
			timer.Tick += (sender, e) =>
			{
				if (isWindowClosed)
				{
					timer.Stop();
					Application.Current.Shutdown();
				}

				OnUpdate();
				Render();
			};
			timer.Start();

			var app = new Application();
			app.Run(_window);
		}

		protected override void CloseWindow()
		{
			_window.Close();
		}

		protected override void SetWindowCaption(string caption)
		{
			_window.Title = caption;
		}
	}
}