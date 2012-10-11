using System.ComponentModel.Composition;
using System.IO;
using Gemini.Framework;
using Gemini.Framework.Services;
using Rasterizr.Studio.Modules.ModelViewer.ViewModels;

namespace Rasterizr.Studio.Modules.TracefileViewer
{
	[Export(typeof(IEditorProvider))]
	public class EditorProvider : IEditorProvider
	{
		public bool Handles(string path)
		{
			return Path.GetExtension(path).ToLower() == ".trace";
		}

		public IDocument Create(string path)
		{
			return new ModelViewerViewModel(path);
		}
	}
}