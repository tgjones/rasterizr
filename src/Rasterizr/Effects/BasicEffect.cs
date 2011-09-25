using System.Runtime.InteropServices;
using Nexus;
using Rasterizr.Core;
using Rasterizr.Core.InputAssembler;
using Rasterizr.Core.ShaderCore;
using Rasterizr.Core.ShaderCore.PixelShader;
using Rasterizr.Core.ShaderCore.VertexShader;

namespace Rasterizr.Effects
{
	public class BasicEffect : Effect
	{
		#region Fields

		private readonly EffectPass _effectPass;

		#endregion

		#region Properties

		public float Alpha { get; set; }
		public ColorRgbF DiffuseColor { get; set; }
		public ColorRgbF SpecularColor { get; set; }
		public float SpecularPower { get; set; }
		public Texture2D Texture { get; set; }
		public bool TextureEnabled { get; set; }
		public bool VertexColorEnabled { get; set; }

		public ColorRgbF AmbientLightColor { get; set; }
		public bool LightingEnabled { get; set; }
		public DirectionalLight DirectionalLight0 { get; private set; }
		public DirectionalLight DirectionalLight1 { get; private set; }
		public DirectionalLight DirectionalLight2 { get; private set; }

		public Matrix3D World { get; set; }
		public Matrix3D View { get; set; }
		public Matrix3D Projection { get; set; }

		#endregion

		#region Constructor

		public BasicEffect(RasterizrDevice device, InputLayout inputLayout)
			: base(device)
		{
			EffectTechnique technique = new EffectTechnique(this);
			_effectPass = new EffectPass(technique);
			technique.Passes.Add(_effectPass);
			Techniques.Add(technique);

			Alpha = 1.0f;
			DirectionalLight0 = new DirectionalLight();
			DirectionalLight1 = new DirectionalLight();
			DirectionalLight2 = new DirectionalLight();
			DiffuseColor = ColorsRgbF.White;
			SpecularColor = ColorsRgbF.White;
			World = Matrix3D.Identity;

			if (inputLayout.ContainsSemantic(Semantics.Normal, 0)
				&& inputLayout.ContainsSemantic(Semantics.TexCoord, 0)
				&& inputLayout.ContainsSemantic(Semantics.Color, 0))
			{
				throw new System.NotImplementedException();
			}
			else if (inputLayout.ContainsSemantic(Semantics.Normal, 0)
				&& inputLayout.ContainsSemantic(Semantics.TexCoord, 0))
			{
				_effectPass.VertexShader = new VertexShaderPnt();
				_effectPass.PixelShader = new PixelShaderNt(this)
				{
					Texture = Texture
				};
			}
			else if (inputLayout.ContainsSemantic(Semantics.Color, 0))
			{
				_effectPass.VertexShader = new VertexShaderPc();
				_effectPass.PixelShader = new PixelShaderC(this);
			}
			else
			{
				throw new System.NotImplementedException();
			}

			EnableDefaultLighting();
		}

		#endregion

		#region Methods

		public void EnableDefaultLighting()
		{
			LightingEnabled = true;

			DirectionalLight0.Direction = new Vector3D(-0.5265408f, -0.5735765f, -0.6275069f);
			DirectionalLight0.DiffuseColor = new ColorRgbF(1f, 0.9607844f, 0.8078432f);
			DirectionalLight0.SpecularColor = new ColorRgbF(1f, 0.9607844f, 0.8078432f);
			DirectionalLight0.Enabled = true;
			DirectionalLight1.Direction = new Vector3D(0.7198464f, 0.3420201f, 0.6040227f);
			DirectionalLight1.DiffuseColor = new ColorRgbF(0.9647059f, 0.7607844f, 0.4078432f);
			DirectionalLight1.SpecularColor = ColorsRgbF.Transparent;
			DirectionalLight1.Enabled = true;
			DirectionalLight2.Direction = new Vector3D(0.4545195f, -0.7660444f, 0.4545195f);
			DirectionalLight2.DiffuseColor = new ColorRgbF(0.3231373f, 0.3607844f, 0.3937255f);
			DirectionalLight2.SpecularColor = new ColorRgbF(0.3231373f, 0.3607844f, 0.3937255f);
			DirectionalLight2.Enabled = true;

			AmbientLightColor = new ColorRgbF(0.05333332f, 0.09882354f, 0.1819608f);
		}

		protected internal override void OnApply()
		{
			IWvpVertexShader vertexShader = (IWvpVertexShader) _effectPass.VertexShader;
			vertexShader.WorldViewProjection = World * View * Projection;

			if (_effectPass.PixelShader is PixelShaderNt)
			{
				PixelShaderNt pixelShader = (PixelShaderNt)_effectPass.PixelShader;
				pixelShader.Eye = Matrix3D.Invert(View).Translation;
			}
		}

		#endregion

		#region Vertex shader

		internal interface IWvpVertexShader
		{
			Matrix3D WorldViewProjection { get; set; }
		}

		internal abstract class BasicEffectVertexShader<TVertexShaderInput, TVertexShaderOutput> : VertexShaderBase<TVertexShaderInput, TVertexShaderOutput>, IWvpVertexShader
			where TVertexShaderInput : new()
			where TVertexShaderOutput : IVertexShaderOutput, new()
		{
			public Matrix3D WorldViewProjection { get; set; }
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct VertexShaderOutputPnt : IVertexShaderOutput
		{
			[Semantic(Semantics.Position)]
			public Point4D Position { get; set; }

			[Semantic(Semantics.Normal)]
			public Vector3D Normal;

			[Semantic(Semantics.TexCoord)]
			public Point2D TextureCoordinate;
		}

		internal class VertexShaderPnt : BasicEffectVertexShader<VertexPositionNormalTexture, VertexShaderOutputPnt>
		{
			public override VertexShaderOutputPnt Execute(VertexPositionNormalTexture vertexShaderInput)
			{
				Point4D position = WorldViewProjection.Transform(vertexShaderInput.Position.ToHomogeneousPoint3D());
				return new VertexShaderOutputPnt
				{
					Position = position,
					Normal = vertexShaderInput.Normal,
					TextureCoordinate = vertexShaderInput.TextureCoordinate,
				};
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct VertexShaderOutputPc : IVertexShaderOutput
		{
			[Semantic(Semantics.Position)]
			public Point4D Position { get; set; }

			[Semantic(Semantics.Color)]
			public ColorF Color;
		}

		internal class VertexShaderPc : BasicEffectVertexShader<VertexPositionColor, VertexShaderOutputPc>
		{
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

		#endregion

		#region Pixel shader

		internal struct ColorPair
		{
			public ColorRgbF Diffuse { get; set; }
			public ColorRgbF Specular { get; set; }
		}

		internal abstract class BasicEffectPixelShader<TPixelShaderInput> : PixelShaderBase<TPixelShaderInput>
			where TPixelShaderInput : new()
		{
			private readonly BasicEffect _effect;

			protected BasicEffect Effect
			{
				get { return _effect; }
			}

			protected BasicEffectPixelShader(BasicEffect effect)
			{
				_effect = effect;
			}

			protected void CalculateDirectionalLight(DirectionalLight light,
				Vector3D eye, Vector3D normal, ref ColorPair result)
			{
				Vector3D l = -light.Direction;
				Vector3D h = Vector3D.Normalize(eye + l);
				LightingCoefficients ret = Lit(Vector3D.Dot(normal, l), Vector3D.Dot(normal, h), _effect.SpecularPower / 64.0f);

				result.Diffuse += light.DiffuseColor * ret.Diffuse;
				result.Specular += light.SpecularColor * ret.Specular;
			}

			protected ColorPair CalculateLighting(Vector3D eye, Vector3D normal)
			{
				ColorPair result = new ColorPair();

				result.Diffuse = _effect.AmbientLightColor;
				result.Specular = ColorsRgbF.Transparent;

				// Directional Light 0
				CalculateDirectionalLight(_effect.DirectionalLight0, eye, normal, ref result);

				// Directional Light 1
				CalculateDirectionalLight(_effect.DirectionalLight1, eye, normal, ref result);

				// Directional Light 2
				CalculateDirectionalLight(_effect.DirectionalLight2, eye, normal, ref result);

				result.Diffuse *= _effect.DiffuseColor;
				//result.Diffuse += _effect.EmissiveColor;
				result.Specular *= _effect.SpecularColor;

				return result;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct PixelShaderInputNt
		{
			[Semantic(Semantics.Normal)]
			public Vector3D Normal;

			[Semantic(Semantics.TexCoord)]
			public Point2D TextureCoordinate;
		}

		internal class PixelShaderNt : BasicEffectPixelShader<PixelShaderInputNt>
		{
			public SamplerState Sampler { get; set; }
			public Texture2D Texture { get; set; }
			public Vector3D Eye { get; set; }

			public PixelShaderNt(BasicEffect effect)
				: base(effect)
			{
				Sampler = new SamplerState();
			}

			public override ColorF Execute(PixelShaderInputNt pin)
			{
				ColorPair lightResult = CalculateLighting(Eye, pin.Normal);

				ColorF texture = (Texture != null)
					? Texture.Sample(Sampler, pin.TextureCoordinate)
					: ColorsF.White;
				ColorF diffuse = texture
					* new ColorF(lightResult.Diffuse, Effect.Alpha);
				ColorF color = diffuse + new ColorF(lightResult.Specular, 0);
				return color;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct PixelShaderInputC
		{
			[Semantic(Semantics.Color)]
			public ColorF Color;
		}

		internal class PixelShaderC : BasicEffectPixelShader<PixelShaderInputC>
		{
			public PixelShaderC(BasicEffect effect)
				: base(effect)
			{
				
			}

			public override ColorF Execute(PixelShaderInputC pin)
			{
				return pin.Color;
			}
		}

		#endregion
	}
}