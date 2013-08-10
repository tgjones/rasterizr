using System.Windows;
using SlimShader.VirtualMachine;
using SlimShader.VirtualMachine.Jitter;

namespace Rasterizr.SampleBrowser
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
        protected override void OnStartup(StartupEventArgs e)
        {
            VirtualMachine.ShaderExecutor = new JitShaderExecutor();
            base.OnStartup(e);
        }
	}
}
