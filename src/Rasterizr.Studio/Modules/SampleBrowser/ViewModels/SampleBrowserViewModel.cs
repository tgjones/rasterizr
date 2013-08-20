using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Gemini.Framework;
using Rasterizr.Studio.Modules.SampleBrowser.Samples;
using Rasterizr.Studio.Modules.SampleBrowser.TechDemos;

namespace Rasterizr.Studio.Modules.SampleBrowser.ViewModels
{
    [Export(typeof(SampleBrowserViewModel))]
    public class SampleBrowserViewModel : Document
    {
        private readonly SampleCategoryViewModel[] _categories;
        public IEnumerable<SampleCategoryViewModel> Categories
        {
            get { return _categories; }
        }

        private SampleCategoryViewModel _selectedCategory;
        public SampleCategoryViewModel SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                NotifyOfPropertyChange(() => SelectedCategory);
            }
        }
        
        [ImportingConstructor]
		public SampleBrowserViewModel(
            [ImportMany] IEnumerable<Lazy<SampleBase, ISampleMetadata>> samples,
			[ImportMany] IEnumerable<Lazy<TechDemoViewModel, ITechDemoMetadata>> techDemos)
		{
		    DisplayName = "Sample Browser";

		    _categories = new[]
		    {
                new SampleCategoryViewModel("Samples", samples.OrderBy(x => x.Metadata.SortOrder).Select(x => new SampleViewModel(x.Value))), 
                new SampleCategoryViewModel("Tech Demos", techDemos.OrderBy(x => x.Metadata.SortOrder).Select(x => x.Value))
		    };
            _selectedCategory = _categories[0];
		}
	}
}