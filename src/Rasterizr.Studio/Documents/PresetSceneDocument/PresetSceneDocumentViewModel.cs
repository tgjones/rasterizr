using System;
using Rasterizr.Studio.Documents.PresetSceneDocument.Scenes;

namespace Rasterizr.Studio.Documents.PresetSceneDocument
{
	public class PresetSceneDocumentViewModel : RenderedDocumentBase
	{
		private readonly Type _sceneType;
		private readonly PresetSceneBase _scene;

		public override string Title
		{
			get { return _sceneType.Name; }
		}

		public PresetSceneDocumentViewModel(Type sceneType)
		{
			_sceneType = sceneType;
			_scene = (PresetSceneBase)Activator.CreateInstance(sceneType);
		}

		protected override void Draw()
		{
			_scene.Draw(Device);
			SwapChain.Present();
		}
	}
}