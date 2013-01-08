using System;
using Rasterizr.Resources;
using SlimShader;
using SlimShader.VirtualMachine.Resources;

namespace Rasterizr.Pipeline
{
	public partial class ShaderResourceView
	{
		private abstract class InnerResourceView : ITexture
		{
			private readonly Format _format;

			protected InnerResourceView(Format format)
			{
				_format = format;
			}

			public abstract DataIndex GetDataIndex(int arrayIndex, int mipSlice, int x, int y, int z, int sampleIndex);
			
			public Number4 Sample(ISampler sampler, Number4 location)
			{
				// TODO: arrayIndex and sampleIndex
				const int arrayIndex = 0;
				const int sampleIndex = 0;

				float u = location.Number0.Float;
				float v = location.Number1.Float;
				float w = location.Number2.Float;

				// TODO: Multiply by dimensions.
				int x = (int) u;
				int y = (int) v;
				int z = (int) w;

				// TODO: Calculate mip slice.
				const int mipSlice = 0;

				return FormatHelper.Convert(_format, GetDataIndex(arrayIndex, mipSlice, x, y, z, sampleIndex)).ToNumber4();
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