using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Rasterizr.SampleBrowser.FrameAnalysis;
using Rasterizr.SampleBrowser.Samples;
using Rasterizr.SampleBrowser.TechDemos;

namespace Rasterizr.SampleBrowser.Framework
{
    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShell
	{
		public override string DisplayName
		{
			get { return "Rasterizr Sample Browser"; }
			set { base.DisplayName = value; }
		}

		[ImportingConstructor]
		public ShellViewModel(
            [ImportMany] IEnumerable<Lazy<SampleBase, ISampleMetadata>> samples,
			[ImportMany] IEnumerable<Lazy<TechDemoViewModel, ITechDemoMetadata>> techDemos)
		{
		    Items.AddRange(new[]
		    {
                new SampleCategoryViewModel("Samples", samples.OrderBy(x => x.Metadata.SortOrder).Select(x => new SampleViewModel(x.Value))), 
                new SampleCategoryViewModel("Tech Demos", techDemos.OrderBy(x => x.Metadata.SortOrder).Select(x => x.Value)),
                new SampleCategoryViewModel("Frame Analyzer", samples.OrderBy(x => x.Metadata.SortOrder).Select(x => new FrameAnalyzerViewModel(x.Value))),
		    });
            ActivateItem(Items[0]);
		}
	}
}