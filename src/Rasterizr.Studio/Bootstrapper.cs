using System;
using Caliburn.Micro;
using Gemini;
using Gemini.Modules.Shell.ViewModels;

namespace Rasterizr.Studio
{
    public class Bootstrapper : AppBootstrapper
    {
        protected override void Configure()
        {
            var defaultLocator = ViewLocator.LocateTypeForModelType;
            ViewLocator.LocateTypeForModelType = (modelType, displayLocation, context) =>
            {
                var viewType = defaultLocator(modelType, displayLocation, context);
                while (viewType == null && modelType != typeof(object))
                {
                    modelType = modelType.BaseType;
                    viewType = defaultLocator(modelType, displayLocation, context);
                }
                return viewType;
            };

            base.Configure();
        }
    }

    public class TestBootstrapper : Caliburn.Micro.Bootstrapper<ShellViewModel>
    {
        
    }
}