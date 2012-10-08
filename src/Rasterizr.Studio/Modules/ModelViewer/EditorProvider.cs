using System.ComponentModel.Composition;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using Meshellator;
using Rasterizr.Studio.Modules.ModelViewer.ViewModels;

namespace Rasterizr.Studio.Modules.ModelViewer
{
	[Export(typeof(IEditorProvider))]
	public class EditorProvider : IEditorProvider
	{
		public bool Handles(string path)
		{
			return MeshellatorLoader.IsSupportedFormat(path);
		}

		public IDocument Create(string path)
		{
			return new ModelViewerViewModel(path);
		}
	}
}