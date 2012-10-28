using System;
using System.Runtime.InteropServices;
using Nexus;
using Nexus.Graphics.Colors;
using Rasterizr.InputAssembler;
using Rasterizr.ShaderCore;
using Rasterizr.ShaderCore.PixelShader;
using Rasterizr.ShaderCore.VertexShader;

namespace Rasterizr.Studio.Modules.DemoSceneViewer.Scenes
{
	public class NoBuffersScene : DemoSceneBase
	{
		public override void Draw(RasterizrDevice device)
		{
			device.ClearDepthBuffer(1);
			device.ClearRenderTarget(ColorsF.Gray);
			device.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
			device.InputAssembler.InputLayout = null;
			device.VertexShader.VertexShader = new VertexShader();
			device.PixelShader.PixelShader = new PixelShader();
			device.Draw(3, 0);
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct VertexShaderInput
		{
			[Semantic(SystemValueType.VertexID)]
			public int VertexID;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct VertexShaderOutput
		{
			[Semantic(SystemValueType.Position)]
			public Point4D Position;

			[Semantic(Semantics.Color)]
			public ColorF Color;
		}

		private class VertexShader : VertexShaderBase<VertexShaderInput, VertexShaderOutput>
		{
			public override VertexShaderOutput Execute(VertexShaderInput input)
			{
				VertexShaderOutput output;

				switch (input.VertexID)
				{
					case 0 :
						output.Position = new Point4D(0, 0.5f, 0.5f, 1);
						break;
					case 2 :
						output.Position = new Point4D(0.5f, -0.5f, 0.5f, 1);
						break;
					case 1 :
						output.Position = new Point4D(-0.5f, -0.5f, 0.5f, 1);
						break;
					default :
						throw new ArgumentException();
				}

				output.Color = new ColorF(
					MathUtility.Clamp(output.Position.X, 0, 1),
					MathUtility.Clamp(output.Position.Y, 0, 1),
					MathUtility.Clamp(output.Position.Z, 0, 1));

				return output;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct PixelShaderInput
		{
			[Semantic(Semantics.Color)]
			public ColorF Color;
		}

		private class PixelShader : PixelShaderBase<PixelShaderInput>
		{
			public override ColorF Execute(PixelShaderInput input)
			{
				return input.Color;
			}
		}
	}
}