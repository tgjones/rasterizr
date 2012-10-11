using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using Gemini.Framework.Services;
using Rasterizr.Studio.Framework;
using Rasterizr.Studio.Modules.DemoSceneViewer.Scenes;
using Rasterizr.Studio.Modules.TracefileViewer.ViewModels;

namespace Rasterizr.Studio.Modules.DemoSceneViewer.ViewModels
{
	public class DemoSceneViewerViewModel : RenderedDocumentBase
	{
		private readonly Type _sceneType;
		private readonly DemoSceneBase _scene;

		public override string DisplayName
		{
			get { return _sceneType.Name; }
		}

		public DemoSceneViewerViewModel(DemoSceneBase scene)
		{
			_sceneType = scene.GetType();
			_scene = scene;
		}

		protected override void Draw()
		{
			_scene.Draw(Device);
			SwapChain.Present();

			_logger.Close();

			var viewModel = new TracefileViewModel("test.trace");
			IoC.BuildUp(viewModel);
			_shell.OpenDocument(viewModel);
		}

		[Import]
		private IShell _shell;
	}
}