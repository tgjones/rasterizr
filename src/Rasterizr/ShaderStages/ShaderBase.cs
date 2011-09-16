using System;

namespace Rasterizr.ShaderStages
{
	public abstract class ShaderBase
	{
		#region Intrinsic functions available to all shader types

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

	public abstract class ShaderBase<TShaderInput, TShaderOutput> : ShaderBase, IShader
		where TShaderInput : new()
	{
		public Type InputType
		{
			get { return typeof(TShaderInput); }
		}

		public Type OutputType
		{
			get { return typeof(TShaderOutput); }
		}

		public object BuildShaderInput()
		{
			return new TShaderInput();
		}

		public abstract TShaderOutput Execute(TShaderInput vertexShaderInput);

		public object Execute(object vertexShaderInput)
		{
			// Cast shader input to strongly-typed TShaderInput.
			var input = (TShaderInput)vertexShaderInput;

			// Execute VertexShader.
			TShaderOutput output = Execute(input);

			// Convert TVertexShaderOutput to VertexShaderOutput.
			return output;
		}
	}
}