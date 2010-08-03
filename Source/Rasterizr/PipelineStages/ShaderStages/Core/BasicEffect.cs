using System.Runtime.InteropServices;
using Nexus;
using Rasterizr.PipelineStages.ShaderStages.PixelShader;
using Rasterizr.PipelineStages.ShaderStages.VertexShader;

namespace Rasterizr.PipelineStages.ShaderStages.Core
{
	public class BasicEffect : Effect
	{
		#region Fields

		private readonly EffectPass _effectPass;

		#endregion

		#region Properties

		public Matrix3D World { get; set; }
		public Matrix3D View { get; set; }
		public Matrix3D Projection { get; set; }
		public Texture2D Texture { get; set; }
		public bool TextureColorEnabled { get; set; }
		public bool VertexColorEnabled { get; set; }

		#endregion

		#region Constructor

		public BasicEffect(RasterizrDevice device)
			: base(device)
		{
			EffectTechnique technique = new EffectTechnique(this);
			_effectPass = new EffectPass(technique);
			technique.Passes.Add(_effectPass);
			Techniques.Add(technique);
		}

		#endregion

		#region Methods

		protected internal override void OnApply()
		{
			IWvpVertexShader vertexShader = null;
			IPixelShader pixelShader = null;
			if (VertexColorEnabled && TextureColorEnabled)
			{
				vertexShader = new VertexShaderPct();
				pixelShader = new PixelShaderCt
				{
					Texture = Texture
				};
			}
			else if (VertexColorEnabled)
			{
				vertexShader = new VertexShaderPc();
				pixelShader = new PixelShaderC();
			}
			else
			{
				throw new System.NotSupportedException();
			}

			vertexShader.WorldViewProjection = World * View * Projection;

			_effectPass.VertexShader = vertexShader;
			_effectPass.PixelShader = pixelShader;
		}

		#endregion

		#region Vertex shader

		internal interface IWvpVertexShader : IVertexShader
		{
			Matrix3D WorldViewProjection { get; set; }
		}

		internal class VertexShaderPnt : VertexShaderBase<VertexPositionNormalTexture, VertexShaderOutputPnt>, IWvpVertexShader
		{
			public Matrix3D WorldViewProjection { get; set; }

			public override VertexShaderOutputPnt Execute(VertexPositionNormalTexture vertexShaderInput)
			{
				Point4D position = WorldViewProjection.Transform(vertexShaderInput.Position.ToHomogeneousPoint3D());
				return new VertexShaderOutputPnt
				{
					Position = position,
					Normal = vertexShaderInput.Normal,
					TextureCoordinate = vertexShaderInput.TextureCoordinate
				};
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct VertexShaderOutputPnt : IVertexShaderOutput
		{
			public Point4D Position { get; set; }
			public Vector3D Normal;
			public Point2D TextureCoordinate;
		}

		internal class VertexShaderPct : VertexShaderBase<VertexPositionColorTexture, VertexShaderOutputPct>, IWvpVertexShader
		{
			public Matrix3D WorldViewProjection { get; set; }

			public override VertexShaderOutputPct Execute(VertexPositionColorTexture vertexShaderInput)
			{
				Point4D position = WorldViewProjection.Transform(vertexShaderInput.Position.ToHomogeneousPoint3D());
				return new VertexShaderOutputPct
				{
					Position = position,
					Color = vertexShaderInput.Color,
					TextureCoordinate = vertexShaderInput.TextureCoordinate
				};
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct VertexShaderOutputPct : IVertexShaderOutput
		{
			public Point4D Position { get; set; }
			public ColorF Color;
			public Point2D TextureCoordinate;
		}

		internal class VertexShaderPc : VertexShaderBase<VertexPositionColor, VertexShaderOutputPc>, IWvpVertexShader
		{
			public Matrix3D WorldViewProjection { get; set; }

			public override VertexShaderOutputPc Execute(VertexPositionColor vertexShaderInput)
			{
				Point4D position = WorldViewProjection.Transform(vertexShaderInput.Position.ToHomogeneousPoint3D());
				return new VertexShaderOutputPc
				{
					Position = position,
					Color = vertexShaderInput.Color
				};
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct VertexShaderOutputPc : IVertexShaderOutput
		{
			public Point4D Position { get; set; }
			public ColorF Color;
		}

		#endregion

		#region Pixel shader

		internal class PixelShaderNt : PixelShaderBase<PixelShaderInputNt>
		{
			public SamplerState Sampler { get; set; }
			public Texture2D Texture { get; set; }

			public PixelShaderNt()
			{
				Sampler = new SamplerState();
			}

			public override Color Execute(PixelShaderInputNt pixelShaderInput)
			{
				return SampleTexture2D(Texture, Sampler, pixelShaderInput.TextureCoordinate).ToRgbColor();
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct PixelShaderInputNt
		{
			public Vector3D Normal;

			[Semantic(Semantics.TexCoord, 0)]
			public Point2D TextureCoordinate;
		}

		internal class PixelShaderCt : PixelShaderBase<PixelShaderInputCt>
		{
			public SamplerState Sampler { get; set; }
			public Texture2D Texture { get; set; }

			public PixelShaderCt()
			{
				Sampler = new SamplerState();
			}

			public override Color Execute(PixelShaderInputCt pixelShaderInput)
			{
				ColorF tex = SampleTexture2D(Texture, Sampler, pixelShaderInput.TextureCoordinate);
				return (tex * pixelShaderInput.Color).ToRgbColor();
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct PixelShaderInputCt
		{
			public ColorF Color;

			[Semantic(Semantics.TexCoord, 0)]
			public Point2D TextureCoordinate;
		}

		internal class PixelShaderC : PixelShaderBase<PixelShaderInputC>
		{
			public override Color Execute(PixelShaderInputC pixelShaderInput)
			{
				return pixelShaderInput.Color.ToRgbColor();
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct PixelShaderInputC
		{
			public ColorF Color;
		}

		#endregion
	}
}