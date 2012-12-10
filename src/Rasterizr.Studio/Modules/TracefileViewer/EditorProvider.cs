using System.ComponentModel.Composition;
using System.IO;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Studio.Modules.TracefileViewer.ViewModels;

namespace Rasterizr.Studio.Modules.TracefileViewer
{
	[Export(typeof(IEditorProvider))]
	public class EditorProvider : IEditorProvider
	{
		public bool Handles(string path)
		{
			return Path.GetExtension(path).ToLower() == ".rlog";
		}

		public IDocument Create(string path)
		{
			using (var reader = new StreamReader(File.OpenRead(path)))
			{
				var tracefile = Tracefile.FromTextReader(reader);
				var result = new TracefileViewerViewModel(path, tracefile);
				IoC.BuildUp(result);
				result.SelectedFrame = result.Frames[0];
				return result;
			}
		}
	}
}