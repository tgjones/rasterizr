using System.Collections.Generic;
using Caliburn.Micro;

namespace Rasterizr.SampleBrowser.Framework
{
    public class SampleCategoryViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public SampleCategoryViewModel(string name, IEnumerable<ISample> samples)
        {
            DisplayName = name;
            Items.AddRange(samples);
            ActivateItem(Items[0]);
        }
    }
}