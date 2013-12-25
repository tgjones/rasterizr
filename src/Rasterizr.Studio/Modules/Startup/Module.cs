using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using Gemini.Framework;
using Rasterizr.Studio.Modules.GraphicsEventList.ViewModels;
using Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels;
using Rasterizr.Studio.Modules.SampleBrowser.ViewModels;

namespace Rasterizr.Studio.Modules.Startup
{
	[Export(typeof(IModule))]
	public class Module : ModuleBase
	{
	    public override IEnumerable<Type> DefaultTools
	    {
	        get
	        {
	            yield return typeof(GraphicsEventListViewModel);
	            yield return typeof(GraphicsPixelHistoryViewModel);
	        }
	    }

	    public override void Initialize()
	    {
	        MainWindow.Title = "Rasterizr Studio";
            Shell.OpenDocument(IoC.Get<SampleBrowserViewModel>());
		}
	}
}