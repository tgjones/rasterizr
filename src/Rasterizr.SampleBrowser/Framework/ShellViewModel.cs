using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Rasterizr.SampleBrowser.Samples;
using Rasterizr.SampleBrowser.TechDemos;

namespace Rasterizr.SampleBrowser.Framework
{
	[Export(typeof(IShell))]
	public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShell
	{
		private int _selectedTabIndex;
		public int SelectedTabIndex
		{
			get { return _selectedTabIndex; }
			set
			{
				_selectedTabIndex = value;
				NotifyOfPropertyChange(() => SelectedTabIndex);
			}
		}

		private readonly IEnumerable<SampleViewModel> _samples;
		public IEnumerable<SampleViewModel> Samples
		{
			get { return _samples; }
		}

		private SampleViewModel _selectedSample;
		public SampleViewModel SelectedSample
		{
			get { return _selectedSample; }
			set
			{
				_selectedSample = value;
				NotifyOfPropertyChange(() => SelectedSample);
				ActivateItem(value);
			}
		}

		private readonly IEnumerable<TechDemoViewModel> _techDemos;
		public IEnumerable<TechDemoViewModel> TechDemos
		{
			get { return _techDemos; }
		}

		private TechDemoViewModel _selectedTechDemo;
		public TechDemoViewModel SelectedTechDemo
		{
			get { return _selectedTechDemo; }
			set
			{
				_selectedTechDemo = value;
				NotifyOfPropertyChange(() => SelectedTechDemo);
				ActivateItem(value);
			}
		}

		public override string DisplayName
		{
			get { return "Rasterizr Sample Browser"; }
			set { base.DisplayName = value; }
		}

		[ImportingConstructor]
		public ShellViewModel([ImportMany] IEnumerable<Lazy<SampleBase, ISampleMetadata>> samples,
			[ImportMany] IEnumerable<Lazy<TechDemoViewModel, ITechDemoMetadata>> techDemos)
		{
			_samples = samples.OrderBy(x => x.Metadata.SortOrder).Select(x => new SampleViewModel(x.Value));
			_selectedSample = _samples.First();

			_techDemos = techDemos.OrderBy(x => x.Metadata.SortOrder).Select(x => x.Value);
			_selectedTechDemo = _techDemos.First();

			ActivateItem(SelectedSample);
		}
	}
}