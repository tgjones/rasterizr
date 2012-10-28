using System.Runtime.InteropServices;
using Nexus;
using Nexus.Graphics.Colors;
using Rasterizr.ShaderCore;
using Rasterizr.ShaderCore.PixelShader;
using Rasterizr.ShaderCore.VertexShader;

namespace Rasterizr.Toolkit.Effects
{
	public class BasicEffect : Effect
	{
		#region Fields

		private readonly EffectPass _effectPass;
		private readonly IShader[] _vertexShaders;
		private readonly IPixelShader[] _pixelShaders;
		private bool _textureEnabled;
		private bool _lightingEnabled;

		#endregion

		#region Properties

		public float Alpha { get; set; }
		public ColorRgbF DiffuseColor { get; set; }
		public ColorRgbF SpecularColor { get; set; }
		public float SpecularPower { get; set; }
		public Texture2D Texture { get; set; }

		public bool TextureEnabled
		{
			get { return _textureEnabled; }
			set
			{
				_textureEnabled = value;
				UpdateActiveShaders();
			}
		}

		public bool VertexColorEnabled { get; set; }

		public ColorRgbF AmbientLightColor { get; set; }

		public bool LightingEnabled
		{
			get { return _lightingEnabled; }
			set
			{
				_lightingEnabled = value;
				UpdateActiveShaders();
			}
		}

		public DirectionalLight DirectionalLight0 { get; private set; }
		public DirectionalLight DirectionalLight1 { get; private set; }
		public DirectionalLight DirectionalLight2 { get; private set; }

		public Matrix3D World { get; set; }
		public Matrix3D View { get; set; }
		public Matrix3D Projection { get; set; }

		#endregion

		#region Constructor

		public BasicEffect(RasterizrDevice device)
			: base(device)
		{
			_vertexShaders = new IShader[]
			{
				new VertexShaderPnt(),
				new VertexShaderPct(),
				new VertexShaderPc()
			};

			_pixelShaders = new IPixelShader[]
			{
				new PixelShaderNt(this),
				new PixelShaderCt(this),
				new PixelShaderC(this)
			};

			var technique = new EffectTechnique(this);
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

			EnableDefaultLighting();

			UpdateActiveShaders();
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

		private void UpdateActiveShaders()
		{
			// Figure out which vertex and pixel shader we should use.
			int shaderIndex;
			if (LightingEnabled)
				shaderIndex = 0;
			else
			{
				if (TextureEnabled)
					shaderIndex = 1;
				else
					shaderIndex = 2;
			}
			_effectPass.VertexShader = _vertexShaders[shaderIndex];
			_effectPass.PixelShader = _pixelShaders[shaderIndex];
		}

		protected internal override void OnApply()
		{
			var vertexShader = (IWvpVertexShader) _effectPass.VertexShader;
			vertexShader.WorldViewProjection = World * View * Projection;

			if (_effectPass.PixelShader is PixelShaderNt)
			{
				var pixelShader = (PixelShaderNt)_effectPass.PixelShader;
				pixelShader.Eye = Matrix3D.Invert(View).Translation;
				pixelShader.Texture = Texture;
			}
			else if (_effectPass.PixelShader is PixelShaderCt)
			{
				var pixelShader = (PixelShaderCt)_effectPass.PixelShader;
				pixelShader.Texture = Texture;
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
		{
			public Matrix3D WorldViewProjection { get; set; }
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct VertexShaderInputPnt
		{
			[Semantic(Semantics.Position)]
			public Point3D Position;

			[Semantic(Semantics.Normal)]
			public Vector3D Normal;

			[Semantic(Semantics.TexCoord)]
			public Point2D TextureCoordinate;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct VertexShaderOutputPnt
		{
			[Semantic(SystemValueType.Position)]
			public Point4D Position;

			[Semantic(Semantics.Normal)]
			public Vector3D Normal;

			[Semantic(Semantics.TexCoord)]
			public Point2D TextureCoordinate;
		}

		internal class VertexShaderPnt : BasicEffectVertexShader<VertexShaderInputPnt, VertexShaderOutputPnt>
		{
			public override VertexShaderOutputPnt Execute(VertexShaderInputPnt vertexShaderInput)
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
		internal struct VertexShaderInputPct
		{
			[Semantic(Semantics.Position)]
			public Point3D Position;

			[Semantic(Semantics.Color)]
			public ColorF Color;

			[Semantic(Semantics.TexCoord)]
			public Point2D TextureCoordinate;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct VertexShaderOutputPct
		{
			[Semantic(SystemValueType.Position)]
			public Point4D Position;

			[Semantic(Semantics.Color)]
			public ColorF Color;

			[Semantic(Semantics.TexCoord)]
			public Point2D TextureCoordinate;
		}

		internal class VertexShaderPct : BasicEffectVertexShader<VertexShaderInputPct, VertexShaderOutputPct>
		{
			public override VertexShaderOutputPct Execute(VertexShaderInputPct vertexShaderInput)
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
		internal struct VertexShaderInputPc
		{
			[Semantic(Semantics.Position)]
			public Point3D Position;

			[Semantic(Semantics.Color)]
			public ColorF Color;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct VertexShaderOutputPc
		{
			[Semantic(SystemValueType.Position)]
			public Point4D Position;

			[Semantic(Semantics.Color)]
			public ColorF Color;
		}

		internal class VertexShaderPc : BasicEffectVertexShader<VertexShaderInputPc, VertexShaderOutputPc>
		{
			public override VertexShaderOutputPc Execute(VertexShaderInputPc vertexShaderInput)
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
			public Texture2D Texture { get; set; }
			public Vector3D Eye { get; set; }
			private SamplerState Sampler { get; set; }

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
		internal struct PixelShaderInputCt
		{
			[Semantic(Semantics.Color)]
			public ColorF Color;

			[Semantic(Semantics.TexCoord)]
			public Point2D TextureCoordinate;
		}

		internal class PixelShaderCt : BasicEffectPixelShader<PixelShaderInputCt>
		{
			public Texture2D Texture { get; set; }
			private SamplerState Sampler { get; set; }

			public PixelShaderCt(BasicEffect effect)
				: base(effect)
			{
				Sampler = SamplerState.PointWrap;
			}

			public override ColorF Execute(PixelShaderInputCt pin)
			{
				ColorF texture = (Texture != null)
					? Texture.Sample(Sampler, pin.TextureCoordinate)
					: ColorsF.White;
				return texture * pin.Color;
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