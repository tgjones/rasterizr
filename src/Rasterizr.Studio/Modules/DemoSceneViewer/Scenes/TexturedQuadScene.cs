using Nexus;
using Nexus.Graphics.Cameras;
using Rasterizr.Core;
using Rasterizr.Core.InputAssembler;
using Rasterizr.Core.ShaderCore;
using Rasterizr.Effects;

namespace Rasterizr.Studio.Modules.DemoSceneViewer.Scenes
{
	public class TexturedQuadScene : DemoSceneBase
	{
		public override void Draw(RasterizrDevice device)
		{
			device.ClearDepthBuffer(1);
			device.ClearRenderTarget(new ColorF(0.8f, 0, 0));

			Camera camera = new PerspectiveCamera
			{
				FieldOfView = MathUtility.PI_OVER_4,
				FarPlaneDistance = 10.0f,
				NearPlaneDistance = 0.1f,
				Position = new Point3D(2, 0, 1.5f),
				LookDirection = Vector3D.Normalize(new Vector3D(-1.2f, 0.4f, -1f))
			};

			var effect = new BasicEffect(device)
			{
				Projection = camera.GetProjectionMatrix(device.Rasterizer.Viewport.AspectRatio),
				View = camera.GetViewMatrix(),
				Texture = Texture2D.CreateCheckerboard(8, 8)
			};

			device.InputAssembler.InputLayout = new InputLayout(VertexPositionColorTexture.InputElements,
				effect.CurrentTechnique.Passes[0].VertexShader);
			device.InputAssembler.Vertices = new[]
			{
				new VertexPositionColorTexture(new Point3D(-1, 0, 0), ColorsF.White, new Point2D(0, 0)),
				new VertexPositionColorTexture(new Point3D(1, 0, 0), ColorsF.White, new Point2D(0, 1)),
				new VertexPositionColorTexture(new Point3D(-1, 1, -1f), ColorsF.White, new Point2D(1, 0)),
				new VertexPositionColorTexture(new Point3D(1, 1, -0.5f), ColorsF.White, new Point2D(1, 1))
			};
			device.InputAssembler.Indices = new Int32Collection(new[] {0, 1, 2, 3});
			device.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleStrip;

			foreach (EffectPass pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				device.DrawIndexed(4, 0, 0);
			}
		}
	}
}