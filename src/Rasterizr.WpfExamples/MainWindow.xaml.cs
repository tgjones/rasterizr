using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Meshellator;
using Microsoft.Win32;
using Nexus;
using Nexus.Graphics;
using Nexus.Graphics.Cameras;
using Rasterizr.Meshellator;
using Rasterizr.OutputMerger;
using Rasterizr.ShaderCore;

namespace Rasterizr.WpfExamples
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private RasterizrDevice _device;
		private SwapChain _swapChain;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			var outputImage = new WriteableBitmap((int)ImageViewport.Width, (int)ImageViewport.Height,
				96, 96, PixelFormats.Bgra32, null);

			_device = new RasterizrDevice();
			_swapChain = new SwapChain(outputImage, (int)ImageViewport.Width, (int)ImageViewport.Height, 1);

			ImageViewport.Source = outputImage;

			_device.Rasterizer.Viewport = new Viewport3D(0, 0, (int)ImageViewport.Width, (int)ImageViewport.Height);
			_device.OutputMerger.RenderTarget = new RenderTargetView(_swapChain.GetBuffer());
		}

		private void mniOpen_Click(object sender, RoutedEventArgs e)
		{
			// Configure open file dialog box
			var dialog = new OpenFileDialog
			{
				DefaultExt = ".obj",
				Filter = "3D models|*.obj;*.3ds;*.nff"
			};

			// Show open file dialog box
			if (dialog.ShowDialog() == true)
			{
				Scene scene = MeshellatorLoader.ImportFromFile(dialog.FileName);
				var model = ModelLoader.FromScene(_device, scene);

				_device.ClearDepthBuffer(1);
				_device.ClearRenderTarget(ColorsF.Gray);

				Camera camera = PerspectiveCamera.CreateFromBounds(scene.Bounds, _device.Rasterizer.Viewport, MathUtility.PI_OVER_4);
				foreach (ModelMesh mesh in model.Meshes)
				{
					var effect = (BasicEffect)mesh.Effect;
					effect.View = camera.GetViewMatrix();
					effect.Projection = camera.GetProjectionMatrix(_device.Rasterizer.Viewport.AspectRatio);
				}
				model.Draw();

				_swapChain.Present();
			}
		}

		private void mniExit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}