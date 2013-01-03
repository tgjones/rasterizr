using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Rasterizr.SampleBrowser.Samples;

namespace Rasterizr.SampleBrowser.Framework
{
	[Export(typeof(IShell))]
	public class ShellViewModel : Conductor<IScreen>, IShell
	{
		private readonly IEnumerable<SampleBase> _samples;
		public IEnumerable<SampleBase> Samples
		{
			get { return _samples; }
		}

		private SampleBase _selectedSample;
		public SampleBase SelectedSample
		{
			get { return _selectedSample; }
			set
			{
				_selectedSample = value;
				NotifyOfPropertyChange(() => SelectedSample);
				ActivateItem(new SampleViewModel(value));
			}
		}

		public override string DisplayName
		{
			get { return "Rasterizr Sample Browser"; }
			set { base.DisplayName = value; }
		}

		[ImportingConstructor]
		public ShellViewModel([ImportMany] IEnumerable<Lazy<SampleBase, ISampleMetadata>> samples)
		{
			_samples = samples.OrderBy(x => x.Metadata.SortOrder).Select(x => x.Value);
			SelectedSample = _samples.First();
		}
	}
}