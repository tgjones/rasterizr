using System.Runtime.InteropServices;
using Nexus;
using Rasterizr.Core;
using Rasterizr.Core.ShaderCore.PixelShader;
using Rasterizr.Core.ShaderCore.VertexShader;

namespace Rasterizr.Effects
{
	public class DepthEffect : Effect
	{
		#region Fields

		private readonly EffectPass _effectPass;

		#endregion

		#region Properties

		public Matrix3D World { get; set; }
		public Matrix3D View { get; set; }
		public Matrix3D Projection { get; set; }

		#endregion

		#region Constructor

		public DepthEffect(RasterizrDevice device)
			: base(device)
		{
			EffectTechnique technique = new EffectTechnique(this);
			_effectPass = new EffectPass(technique);
			technique.Passes.Add(_effectPass);
			Techniques.Add(technique);

			World = Matrix3D.Identity;

			_effectPass.VertexShader = new DepthEffectVertexShader();
			_effectPass.PixelShader = new DepthEffectPixelShader();
		}

		#endregion

		#region Methods

		protected internal override void OnApply()
		{
			IWvpVertexShader vertexShader = (IWvpVertexShader) _effectPass.VertexShader;
			vertexShader.WorldViewProjection = World * View * Projection;
		}

		#endregion

		#region Vertex shader

		internal interface IWvpVertexShader
		{
			Matrix3D WorldViewProjection { get; set; }
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct DepthEffectVertexShaderOutput : IVertexShaderOutput
		{
			public Point4D Position { get; set; }
			public float Depth;
		}

		internal class DepthEffectVertexShader : VertexShaderBase<VertexPositionNormalTexture, DepthEffectVertexShaderOutput>, IWvpVertexShader
		{
			public Matrix3D WorldViewProjection { get; set; }

			public override DepthEffectVertexShaderOutput Execute(VertexPositionNormalTexture vertexShaderInput)
			{
				Point4D position = WorldViewProjection.Transform(vertexShaderInput.Position.ToHomogeneousPoint3D());
				return new DepthEffectVertexShaderOutput
				{
					Position = position,
					Depth = position.Z / 4000.0f
				};
			}
		}

		#endregion

		#region Pixel shader

		[StructLayout(LayoutKind.Sequential)]
		internal struct DepthEffectPixelShaderInput
		{
			public float Depth;
		}

		internal class DepthEffectPixelShader : PixelShaderBase<DepthEffectPixelShaderInput>
		{
			public override ColorF Execute(DepthEffectPixelShaderInput pin)
			{
				return new ColorF(pin.Depth);
			}
		}

		#endregion
	}
}