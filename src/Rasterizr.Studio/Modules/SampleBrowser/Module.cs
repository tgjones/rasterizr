using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Results;
using Gemini.Modules.MainMenu.Models;
using Rasterizr.Studio.Modules.SampleBrowser.ViewModels;
using SlimShader.VirtualMachine;
using SlimShader.VirtualMachine.Jitter;

namespace Rasterizr.Studio.Modules.SampleBrowser
{
    [Export(typeof(IModule))]
    public class Module : ModuleBase
    {
        public override void Initialize()
        {
            VirtualMachine.ShaderExecutor = new JitShaderExecutor();

            var fileMenuItem = Shell.MainMenu.First(mi => mi.Name == "File");
            fileMenuItem.Children.Insert(0, new MenuItem("Open Sample Browser", OpenSampleBrowser));
        }

        private static IEnumerable<IResult> OpenSampleBrowser()
        {
            yield return Show.Document(IoC.Get<SampleBrowserViewModel>());
        }
    }
}