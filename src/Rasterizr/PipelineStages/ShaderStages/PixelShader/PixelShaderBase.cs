using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nexus;
using Rasterizr.PipelineStages.Rasterizer;
using Rasterizr.PipelineStages.ShaderStages.Core;
using Rasterizr.PipelineStages.TriangleSetup;

namespace Rasterizr.PipelineStages.ShaderStages.PixelShader
{
	public abstract class PixelShaderBase<TPixelShaderInput> : ShaderBase, IPixelShader
		where TPixelShaderInput : new()
	{
		/// <summary>
		/// The presence of this field means the PixelShaderBase class is not thread-safe.
		/// </summary>
		private readonly Dictionary<SamplerState, TextureCoordinatePartialDifferentials> _partialDifferentials = new Dictionary<SamplerState,TextureCoordinatePartialDifferentials>();

		private readonly Dictionary<string, SemanticAttribute> _semanticAttributes = new Dictionary<string, SemanticAttribute>();
		private readonly Dictionary<string, FieldInfo> _fieldInfos = new Dictionary<string, FieldInfo>();

		public abstract ColorF Execute(TPixelShaderInput pixelShaderInput);

		public ColorF Execute(Fragment fragment)
		{
			_partialDifferentials.Clear();

			// Convert VertexShaderInput to TVertexShaderInput.
			TPixelShaderInput input = BuildTypedPixelShaderInput(fragment);

			// Execute VertexShader.
			return Execute(input);
		}

		private TPixelShaderInput BuildTypedPixelShaderInput(Fragment fragment)
		{
			TPixelShaderInput typedInput = new TPixelShaderInput();
			object wrapper = typedInput;
			foreach (InterpolatedVertexAttribute vertexAttribute in fragment.Attributes)
			{
				FieldInfo fieldInfo = GetFieldInfo(vertexAttribute.Name);
				if (fieldInfo == null) // i.e. a system-value semantic
					continue;

				SemanticAttribute semanticAttribute = GetSemanticAttribute(fieldInfo, vertexAttribute.Name);
				if (semanticAttribute != null)
				{
					SamplerState samplerState = (SamplerState) GetType().GetProperties()
						.Where(pi => pi.PropertyType == typeof(SamplerState))
						.ElementAt(semanticAttribute.Index)
						.GetValue(this, null);
					_partialDifferentials[samplerState] = new TextureCoordinatePartialDifferentials
					{
						DuDx = ((Point2D) vertexAttribute.DValueDx.Value).X,
						DvDx = ((Point2D) vertexAttribute.DValueDx.Value).Y,
						DuDy = ((Point2D) vertexAttribute.DValueDy.Value).X,
						DvDy = ((Point2D) vertexAttribute.DValueDy.Value).Y
					};
				}
				fieldInfo.SetValue(wrapper, vertexAttribute.Value.Value);
			}
			typedInput = (TPixelShaderInput) wrapper;
			return typedInput;
		}

		private FieldInfo GetFieldInfo(string name)
		{
			if (!_fieldInfos.ContainsKey(name))
				_fieldInfos.Add(name, typeof(TPixelShaderInput).GetField(name));
			return _fieldInfos[name];
		}

		private SemanticAttribute GetSemanticAttribute(FieldInfo fieldInfo, string name)
		{
			if (!_semanticAttributes.ContainsKey(name))
			{
				SemanticAttribute[] semanticAttributes = (SemanticAttribute[]) fieldInfo.GetCustomAttributes(typeof(SemanticAttribute), false);
				if (semanticAttributes != null && semanticAttributes.Length > 0)
					_semanticAttributes.Add(name, semanticAttributes[0]);
				else
					_semanticAttributes.Add(name, null);
			}
			return _semanticAttributes[name];
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