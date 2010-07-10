using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.Rasterizer;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.ShaderStages.Core;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.TriangleSetup;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.ShaderCore;
using Nexus;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.PixelShader
{
	public abstract class PixelShaderBase<TPixelShaderInput> : ShaderBase, IPixelShader
		where TPixelShaderInput : new()
	{
		/// <summary>
		/// The presence of this field means the PixelShaderBase class is not thread-safe.
		/// </summary>
		private readonly Dictionary<SamplerState, TextureCoordinatePartialDifferentials> _partialDifferentials = new Dictionary<SamplerState,TextureCoordinatePartialDifferentials>();

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
				FieldInfo fieldInfo = typeof(TPixelShaderInput).GetField(vertexAttribute.Name);
				SemanticAttribute[] semanticAttributes = (SemanticAttribute[]) fieldInfo.GetCustomAttributes(typeof(SemanticAttribute), false);
				if (semanticAttributes != null && semanticAttributes.Length > 0)
				{
					SemanticAttribute semanticAttribute = semanticAttributes[0];
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

		#region Intrinsic shader functions

		protected ColorF SampleTexture2D(Texture2D texture, SamplerState samplerState, Point2D textureCoordinates)
		{
			return texture.Sample(samplerState, textureCoordinates.X, textureCoordinates.Y,
				_partialDifferentials[samplerState]);
		}

		#endregion
	}
}