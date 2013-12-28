using Rasterizr.Resources;
using SlimShader.VirtualMachine.Resources;

namespace Rasterizr.Pipeline
{
	public partial class ShaderResourceView
	{
		private class Texture2DView : InnerResourceView
		{
			private readonly Texture2D.Texture2DSubresource[] _subresources;

            public override int MipMapCount
            {
                get { return _subresources.Length; }
            }

			public Texture2DView(Texture2D resource, ShaderResourceViewDescription.Texture2DResource description)
			{
				_subresources = new Texture2D.Texture2DSubresource[description.MipLevels];
				for (int i = description.MostDetailedMip; i < description.MostDetailedMip + description.MipLevels; i++)
					_subresources[i] = resource.GetSubresource(0, i);
			}

		    public override ITextureMipMap GetMipMap(int arraySlice, int mipLevel)
		    {
		        return _subresources[mipLevel];
		    }
		}
	}
}