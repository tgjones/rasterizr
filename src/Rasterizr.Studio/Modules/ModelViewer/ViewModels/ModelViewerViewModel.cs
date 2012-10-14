using System.IO;
using Meshellator;
using Nexus;
using Nexus.Graphics.Cameras;
using Nexus.Graphics.Colors;
using Rasterizr.Effects;
using Rasterizr.Meshellator;
using Rasterizr.Studio.Framework;

namespace Rasterizr.Studio.Modules.ModelViewer.ViewModels
{
	public class ModelViewerViewModel : RenderedDocumentBase
	{
		private readonly string _fileName;
		private readonly Scene _scene;
		private readonly Model _model;

		public override string DisplayName
		{
			get { return Path.GetFileName(_fileName); }
		}

		public ModelViewerViewModel(string fileName)
		{
			_fileName = fileName;
			_scene = MeshellatorLoader.ImportFromFile(fileName);
			_model = ModelLoader.FromScene(Device, _scene);
		}

		protected override void Draw()
		{
			Device.ClearDepthBuffer(1);
			Device.ClearRenderTarget(ColorsF.Gray);

			Camera camera = PerspectiveCamera.CreateFromBounds(_scene.Bounds, Device.Rasterizer.Viewport, MathUtility.PI_OVER_4,
				MathUtility.PI_OVER_4, -MathUtility.PI_OVER_4 / 2.0f);
			foreach (ModelMesh mesh in _model.Meshes)
			{
				var effect = (BasicEffect)mesh.Effect;
				effect.View = camera.GetViewMatrix();
				effect.Projection = camera.GetProjectionMatrix(Device.Rasterizer.Viewport.AspectRatio);
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

			SwapChain.Present();
		}
	}
}