using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace Rasterizr.Studio.Modules.DemoSceneViewer
{
	public class OpenWindowResult : IResult
	{
		private readonly IScreen _screen;

		[Import]
		private IWindowManager _windowManager;

		public OpenWindowResult(IScreen screen)
		{
			_screen = screen;
		}

		public void Execute(ActionExecutionContext context)
		{
			_windowManager.ShowWindow(_screen);
			OnCompleted(new ResultCompletionEventArgs());
		}

		public event EventHandler<ResultCompletionEventArgs> Completed;

		private void OnCompleted(ResultCompletionEventArgs e)
		{
			EventHandler<ResultCompletionEventArgs> handler = Completed;
			if (handler != null) handler(this, e);
		}
	}
}