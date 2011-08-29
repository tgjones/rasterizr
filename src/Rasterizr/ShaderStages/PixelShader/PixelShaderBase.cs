using System;
using System.Collections.Generic;
using Nexus;
using Rasterizr.Rasterizer;
using Rasterizr.ShaderStages.Core;

namespace Rasterizr.ShaderStages.PixelShader
{
	public abstract class PixelShaderBase<TPixelShaderInput> : ShaderBase, IPixelShader
		where TPixelShaderInput : IPixelShaderInput, new()
	{
		/// <summary>
		/// The presence of this field means the PixelShaderBase class is not thread-safe.
		/// </summary>
		private readonly Dictionary<SamplerState, TextureCoordinatePartialDifferentials> _partialDifferentials = new Dictionary<SamplerState,TextureCoordinatePartialDifferentials>();

		public abstract ColorF Execute(TPixelShaderInput pixelShaderInput);

		public IPixelShaderInput BuildPixelShaderInput()
		{
			return new TPixelShaderInput();
		}

		public ColorF Execute(Fragment fragment)
		{
			_partialDifferentials.Clear();

			// Execute pixel shader.
			var input = (TPixelShaderInput)fragment.PixelShaderInput;
			return Execute(input);
		}

		#region Intrinsic shader functions

		protected ColorF SampleTexture2D(Texture2D texture, SamplerState samplerState, Point2D textureCoordinates)
		{
			if (texture != null)
				return texture.Sample(samplerState, textureCoordinates.X, textureCoordinates.Y,
					_partialDifferentials[samplerState]);
			return ColorsF.Transparent;
		}

		protected struct LightingCoefficients
		{
			public float Ambient;
			public float Diffuse;
			public float Specular;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nDotL">The dot product of the normalized surface normal and the light vector.</param>
		/// <param name="nDotH">The dot product of the half-angle vector and the surface normal.</param>
		/// <param name="m">A specular exponent.</param>
		/// <returns>The lighting coefficient vector.</returns>
		protected LightingCoefficients Lit(float nDotL, float nDotH, float m)
		{
			LightingCoefficients result = new LightingCoefficients();
			result.Ambient = 1.0f;
			result.Diffuse = Math.Max(0, nDotL);
			result.Specular = (nDotL < 0) || (nDotH < 0) ? 0 : (nDotH * m);
			return result;
		}

		#endregion
	}
}