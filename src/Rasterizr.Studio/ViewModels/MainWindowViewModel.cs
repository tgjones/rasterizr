using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Meshellator;
using Microsoft.Win32;
using Nexus;
using Nexus.Graphics;
using Nexus.Graphics.Cameras;
using Rasterizr.Meshellator;
using Rasterizr.OutputMerger;
using Rasterizr.ShaderCore;

namespace Rasterizr.Demo.ViewModels
{
	public class MainWindowViewModel
	{
		private readonly RasterizrDevice _device;
		private SwapChain _swapChain;
		private Scene _scene;
		private Model _model;

		#region Properties

		private WriteableBitmap _outputBitmap;
		public WriteableBitmap OutputBitmap
		{
			get { return _outputBitmap; }
			set
			{
				_outputBitmap = value;
				RecreateSwapChain();
			}
		}

		#endregion

		#region Commands

		#region Open

		private RelayCommand _openCommand;
		public ICommand OpenCommand
		{
			get
			{
				return _openCommand ?? (_openCommand = new RelayCommand(x =>
				{
					// Configure open file dialog box
					var dialog = new OpenFileDialog
					{
						DefaultExt = ".obj",
						Filter = "3D models|*.obj;*.3ds;*.nff;*.ply"
					};

					// Show open file dialog box
					if (dialog.ShowDialog() == true)
					{
						_scene = MeshellatorLoader.ImportFromFile(dialog.FileName);
						_model = ModelLoader.FromScene(_device, _scene);

						Draw();
					}
				}));
			}
		}

		#endregion 

		#region Exit

		private RelayCommand _exitCommand;
		public ICommand ExitCommand
		{
			get { return _exitCommand ?? (_exitCommand = new RelayCommand(x => Application.Current.Shutdown())); }
		}

		#endregion 

		#endregion

		public MainWindowViewModel()
		{
			_device = new RasterizrDevice();
		}

		private void RecreateSwapChain()
		{
			_swapChain = new SwapChain(OutputBitmap, (int) OutputBitmap.Width, (int) OutputBitmap.Height, 1);
			_device.Rasterizer.Viewport = new Viewport3D(0, 0, (int) OutputBitmap.Width, (int) OutputBitmap.Height);
			_device.OutputMerger.RenderTarget = new RenderTargetView(_swapChain.GetBuffer());
		}

		private void Draw()
		{
			_device.ClearDepthBuffer(1);
			_device.ClearRenderTarget(ColorsF.Gray);

			Camera camera = PerspectiveCamera.CreateFromBounds(_scene.Bounds, _device.Rasterizer.Viewport, MathUtility.PI_OVER_4,
				MathUtility.PI_OVER_4, -MathUtility.PI_OVER_4 / 2.0f);
			foreach (ModelMesh mesh in _model.Meshes)
			{
				var effect = (BasicEffect)mesh.Effect;
				effect.View = camera.GetViewMatrix();
				effect.Projection = camera.GetProjectionMatrix(_device.Rasterizer.Viewport.AspectRatio);
			}

			foreach (ModelMesh mesh in _model.Meshes)
			{
				var effect = (BasicEffect)mesh.Effect;
				if (effect.Alpha < 1.0f)
					continue;
				mesh.Draw();
			}
			foreach (ModelMesh mesh in _model.Meshes)
			{
				var effect = (BasicEffect)mesh.Effect;
				if (effect.Alpha == 1.0f)
					continue;
				mesh.Draw();
			}

			_swapChain.Present();
		}
	}
}