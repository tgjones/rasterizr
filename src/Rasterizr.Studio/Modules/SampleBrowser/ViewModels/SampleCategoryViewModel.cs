using System.Collections.Generic;
using Caliburn.Micro;

namespace Rasterizr.Studio.Modules.SampleBrowser.ViewModels
{
    public class SampleCategoryViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public SampleCategoryViewModel(string name, IEnumerable<ISample> samples)
        {
            ((IActivate) this).Activate();

            DisplayName = name;
            Items.AddRange(samples);
            ActivateItem(Items[0]);
        }
    }
}