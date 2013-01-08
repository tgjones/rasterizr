using System;
using Rasterizr.Math;
using Rasterizr.Resources;
using SlimShader;
using SlimShader.VirtualMachine.Resources;

namespace Rasterizr.Pipeline
{
	public partial class ShaderResourceView
	{
		private abstract class InnerResourceView : ITexture
		{
			public abstract Color4F GetDataIndex(SamplerStateDescription sampler, float u, float v, float w);
			
			public Number4 Sample(ISampler sampler, Number4 location)
			{
				float u = location.Number0.Float;
				float v = location.Number1.Float;
				float w = location.Number2.Float;

				return GetDataIndex(((SamplerStateDescription) sampler.Description), u, v, w).ToNumber4();
			}

			public static InnerResourceView Create(Resource resource, ShaderResourceViewDescription description, Format format)
			{
				switch (description.Dimension)
				{
					case ShaderResourceViewDimension.Buffer:
						throw new NotImplementedException();
					case ShaderResourceViewDimension.Texture1D:
						throw new NotImplementedException();
					case ShaderResourceViewDimension.Texture1DArray:
						throw new NotImplementedException();
					case ShaderResourceViewDimension.Texture2D:
						return new Texture2DView((Texture2D) resource, description.Texture2D, format);
					case ShaderResourceViewDimension.Texture2DArray:
						throw new NotImplementedException();
					case ShaderResourceViewDimension.Texture2DMultisampled:
						throw new NotImplementedException();
					case ShaderResourceViewDimension.Texture2DMultisampledArray:
						throw new NotImplementedException();
					case ShaderResourceViewDimension.Texture3D:
						throw new NotImplementedException();
					case ShaderResourceViewDimension.TextureCube:
						throw new NotImplementedException();
					case ShaderResourceViewDimension.TextureCubeArray:
						throw new NotImplementedException();
					case ShaderResourceViewDimension.ExtendedBuffer:
						throw new NotImplementedException();
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}