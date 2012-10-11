using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Menus;
using Gemini.Framework.Results;
using Rasterizr.Studio.Modules.DemoSceneViewer.Scenes;
using Rasterizr.Studio.Modules.DemoSceneViewer.ViewModels;

namespace Rasterizr.Studio.Modules.DemoSceneViewer
{
	[Export(typeof(IModule))]
	public class Module : ModuleBase
	{
		public override void Initialize()
		{
			Shell.Title = "Meshellator Viewer";

			var fileMenuItem = Shell.MainMenu.First(mi => mi.Name == "File");
			var demoSceneMenuItem = new MenuItem("Demo Scenes");
			fileMenuItem.Children.Insert(0, demoSceneMenuItem);
			fileMenuItem.Children.Insert(1, new MenuItemSeparator());

			demoSceneMenuItem.Add(
				new MenuItem("Simple Triangle", OpenDemoScene<SimpleTriangleScene>),
				new MenuItem("Textured Quad", OpenDemoScene<TexturedQuadScene>),
				new MenuItem("Affine Texture Mapping", OpenDemoScene<AffineTextureMappingScene>),
				new MenuItem("Perspective-Correct Texture Mapping", OpenDemoScene<PerspectiveCorrectTextureMappingScene>),
				new MenuItem("Texture Mip-Mapping", OpenDemoScene<TextureMipMappingScene>),
				new MenuItem("No Buffers", OpenDemoScene<NoBuffersScene>));
		}

		private static IEnumerable<IResult> OpenDemoScene<TScene>()
			where TScene : DemoSceneBase, new()
		{
			var viewModel = new DemoSceneViewerViewModel(new TScene());
			IoC.BuildUp(viewModel);
			yield return new OpenWindowResult(viewModel);
		}
	}
}