using System.ComponentModel.Composition;
using System.IO;
using Gemini.Framework;
using Rasterizr.Diagnostics.Logging;

namespace Rasterizr.Studio.Modules.TracefileViewer.ViewModels
{
	public class TracefileViewModel : Document
	{
		private readonly string _fileName;
		private readonly Tracefile _tracefile;

		[Import]
		private ITracefileService _tracefileService;

		public override string DisplayName
		{
			get { return Path.GetFileName(_fileName); }
		}

		public Tracefile Tracefile
		{
			get { return _tracefile; }
		}

		protected override void OnActivate()
		{
			_tracefileService.NotifyTracefileFrameChanged(new TracefileFrameViewModel(_tracefile.Frames[0]));
			base.OnActivate();
		}

		public TracefileViewModel(string fileName)
		{
			_fileName = fileName;
			_tracefile = Tracefile.FromFile(fileName);
		}
	}
}