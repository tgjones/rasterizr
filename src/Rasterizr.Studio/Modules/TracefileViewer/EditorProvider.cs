using System.ComponentModel.Composition;
using System.IO;
using Gemini.Framework;
using Gemini.Framework.Services;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Studio.Modules.GraphicsDebugging;
using Rasterizr.Studio.Modules.TracefileViewer.ViewModels;

namespace Rasterizr.Studio.Modules.TracefileViewer
{
	[Export(typeof(IEditorProvider))]
	public class EditorProvider : IEditorProvider
	{
		private readonly ISelectionService _selectionService;

		[ImportingConstructor]
		public EditorProvider(ISelectionService selectionService)
		{
			_selectionService = selectionService;
		}

		public bool Handles(string path)
		{
			return Path.GetExtension(path).ToLower() == ".rlog";
		}

		public IDocument Create(string path)
		{
			using (var reader = new StreamReader(File.OpenRead(path)))
			{
				var tracefile = Tracefile.FromTextReader(reader);
				return new TracefileViewerViewModel(_selectionService, path, tracefile);
			}
		}
	}
}