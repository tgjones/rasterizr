using System;
using Rasterizr.Studio.Framework;
using Rasterizr.Studio.Modules.DemoSceneViewer.Scenes;

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
		}
	}
}