using System.Runtime.InteropServices;
using Nexus;
using Nexus.Graphics.Cameras;
using Rasterizr.Core;
using Rasterizr.Core.InputAssembler;
using Rasterizr.Core.ShaderCore;
using Rasterizr.Core.ShaderCore.PixelShader;
using Rasterizr.Core.ShaderCore.VertexShader;

namespace Rasterizr.Studio.Modules.DemoSceneViewer.Scenes
{
	public class PerspectiveCorrectTextureMappingScene : DemoSceneBase
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
				Position = new Point3D(0, 0.5f, 2),
				LookDirection = Vector3D.Forward
			};

			var vertexShader = new VertexShader
			{
				WorldViewProjection = camera.GetViewMatrix() * camera.GetProjectionMatrix(device.Rasterizer.Viewport.AspectRatio)
			};
			device.InputAssembler.InputLayout = new InputLayout(VertexPositionColorTexture.InputElements, vertexShader);
			device.InputAssembler.Vertices = new[]
			{
				new VertexPositionColorTexture(new Point3D(-1, 0, 0), ColorsF.White, new Point2D(0, 0)),
				new VertexPositionColorTexture(new Point3D(1, 0, 0), ColorsF.White, new Point2D(0, 1)),
				new VertexPositionColorTexture(new Point3D(-1, 1, -2), ColorsF.White, new Point2D(1, 0)),
				new VertexPositionColorTexture(new Point3D(1, 1, -2), ColorsF.White, new Point2D(1, 1))
			};
			device.InputAssembler.Indices = new Int32Collection(new[] {0, 1, 2, 3});
			device.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleStrip;

			device.VertexShader.VertexShader = vertexShader;
			device.PixelShader.PixelShader = new PixelShader
			{
				Texture = Texture2D.CreateCheckerboard(8, 8)
			};

			device.DrawIndexed(4, 0, 0);
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct VertexShaderOutput
		{
			[Semantic(SystemValueType.Position)]
			public Point4D Position;

			[Semantic(Semantics.TexCoord)]
			public Point2D TextureCoordinate;
		}

		private class VertexShader : VertexShaderBase<VertexPositionColorTexture, VertexShaderOutput>
		{
			public Matrix3D WorldViewProjection { get; set; }

			public override VertexShaderOutput Execute(VertexPositionColorTexture vertexShaderInput)
			{
				Point4D position = WorldViewProjection.Transform(vertexShaderInput.Position.ToHomogeneousPoint3D());
				return new VertexShaderOutput
				{
					Position = position,
					TextureCoordinate = vertexShaderInput.TextureCoordinate
				};
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct PixelShaderInput
		{
			[Semantic(Semantics.TexCoord)]
			public Point2D TextureCoordinate;
		}

		private class PixelShader : PixelShaderBase<PixelShaderInput>
		{
			public Texture2D Texture { get; set; }
			private SamplerState Sampler { get; set; }

			public PixelShader()
			{
				Sampler = SamplerState.PointWrap;
			}

			public override ColorF Execute(PixelShaderInput pin)
			{
				return Texture.Sample(Sampler, pin.TextureCoordinate);
			}
		}
	}
}