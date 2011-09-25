using Nexus;
using Nexus.Graphics.Cameras;
using Rasterizr.Core;
using Rasterizr.Core.InputAssembler;
using Rasterizr.Core.ShaderCore;
using Rasterizr.Effects;

namespace Rasterizr.Studio.Documents.PresetSceneDocument.Scenes
{
	public class TexturedQuadScene : PresetSceneBase
	{
		public override void Draw(RasterizrDevice device)
		{
			device.ClearDepthBuffer(1);
			device.ClearRenderTarget(ColorsF.Gray);

			Camera camera = new PerspectiveCamera
			{
				FieldOfView = MathUtility.PI_OVER_4,
				FarPlaneDistance = 100.0f,
				NearPlaneDistance = 1.0f,
				Position = new Point3D(1, 0, 2),
				LookDirection = Vector3D.Normalize(new Vector3D(-0.4f, 0.2f, -0.8f))
			};

			device.InputAssembler.InputLayout = VertexPositionColorTexture.InputLayout;
			device.InputAssembler.Vertices = new[]
			{
				new VertexPositionColorTexture(new Point3D(-1, 0, 0), ColorsF.White, new Point2D(0, 0)),
				new VertexPositionColorTexture(new Point3D(1, 0, 0), ColorsF.White, new Point2D(0, 1)),
				new VertexPositionColorTexture(new Point3D(-1, 1, -1), ColorsF.White, new Point2D(1, 0)),
				new VertexPositionColorTexture(new Point3D(1, 1, -1), ColorsF.White, new Point2D(1, 1))
			};
			device.InputAssembler.Indices = new Int32Collection(new[] {0, 1, 2, 3});
			device.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleStrip;

			var effect = new BasicEffect(device, device.InputAssembler.InputLayout)
			{
				Projection = camera.GetProjectionMatrix(device.Rasterizer.Viewport.AspectRatio),
				View = camera.GetViewMatrix(),
				Texture = new Texture2D("pack://application:,,,/Assets/Textures/Checkerboard.png")
			};

			foreach (EffectPass pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				device.Draw();
			}
		}
	}
}