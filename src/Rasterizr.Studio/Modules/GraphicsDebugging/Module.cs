using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Menus;
using Gemini.Framework.Results;
using Rasterizr.Studio.Modules.GraphicsEventList.ViewModels;

namespace Rasterizr.Studio.Modules.GraphicsDebugging
{
	[Export(typeof(IModule))]
	public class Module : ModuleBase
	{
		public override void Initialize()
		{
			var viewMenuItem = Shell.MainMenu.First(mi => mi.Name == "View");
			var graphicsMenuItem = new MenuItem("Graphics");
			viewMenuItem.Children.Add(graphicsMenuItem);
			viewMenuItem.Children.Add(new MenuItemSeparator());

			graphicsMenuItem.Add(new MenuItem("Graphics Event List", OpenTool<GraphicsEventListViewModel>));
		}

		private static IEnumerable<IResult> OpenTool<TTool>()
			where TTool : Tool
		{
			yield return Show.Tool(IoC.Get<TTool>());
		}
	}
}