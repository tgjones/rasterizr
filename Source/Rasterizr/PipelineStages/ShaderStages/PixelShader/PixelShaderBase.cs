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

		public abstract Color Execute(TPixelShaderInput pixelShaderInput);

		public Color Execute(Fragment fragment)
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
			return texture.Sample(samplerState, textureCoordinates.X, textureCoordinates.Y,
				_partialDifferentials[samplerState]);
		}

		#endregion
	}
}