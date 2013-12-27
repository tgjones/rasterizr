using Rasterizr.Resources;
using SlimShader.VirtualMachine.Resources;

namespace Rasterizr.Pipeline
{
	public partial class ShaderResourceView
	{
		private class Texture2DArrayView : InnerResourceView
		{
            private readonly int _mipMapCount;
            private readonly Texture2D.Texture2DSubresource[] _subresources;

		    public override TextureDimension Dimension
            {
                get { return TextureDimension.Texture2D; }
            }

            public override int MipMapCount
            {
                get { return _mipMapCount; }
            }

            public Texture2DArrayView(Texture2D resource, ShaderResourceViewDescription.Texture2DArrayResource description)
            {
                _mipMapCount = description.MipLevels;

				_subresources = new Texture2D.Texture2DSubresource[description.MipLevels * description.ArraySize];

                int counter = 0;
                for (int i = description.FirstArraySlice; i < description.FirstArraySlice + description.ArraySize; i++)
				    for (int j = description.MostDetailedMip; j < description.MostDetailedMip + description.MipLevels; j++)
					    _subresources[counter++] = resource.GetSubresource(i, j);
			}

		    public override ITextureMipMap GetMipMap(int level)
		    {
		        return _subresources[level];
		    }
		}
	}
}