using System;
using Rasterizr.Resources;
using SlimShader.VirtualMachine.Resources;

namespace Rasterizr.Pipeline
{
	public partial class ShaderResourceView
	{
		private abstract class InnerResourceView : ITexture
		{
			public static InnerResourceView Create(Resource resource, ShaderResourceViewDescription description)
			{
				switch (description.Dimension)
				{
					case ShaderResourceViewDimension.Texture1D:
						throw new NotImplementedException();
					case ShaderResourceViewDimension.Texture1DArray:
						throw new NotImplementedException();
					case ShaderResourceViewDimension.Texture2D:
						return new Texture2DView((Texture2D) resource, description.Texture2D);
					case ShaderResourceViewDimension.Texture2DArray:
                        return new Texture2DArrayView((Texture2D) resource, description.Texture2DArray);
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
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

            public abstract int MipMapCount { get; }
            public abstract ITextureMipMap GetMipMap(int arraySlice, int mipLevel);
		}
	}
}