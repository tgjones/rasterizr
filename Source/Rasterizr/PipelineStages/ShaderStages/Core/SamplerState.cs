using Nexus;

namespace Rasterizr.PipelineStages.ShaderStages.Core
{
	public class SamplerState
	{
		public TextureAddressMode AddressU { get; set; }
		public TextureAddressMode AddressV { get; set; }

		public ColorF BorderColor { get; set; }

		public TextureFilter MagFilter { get; set; }

		public TextureFilter MinFilter { get; set; }

		/// <summary>
		/// MipFilter is only used in when minifying (i.e. the pixel being rendered is smaller than a texel).
		/// In conjunction with the MinFilter, there are 4 combinations of filters.
		/// </summary>
		public TextureMipMapFilter MipFilter { get; set; }

		public SamplerState()
		{
			AddressU = TextureAddressMode.Wrap;
			AddressV = TextureAddressMode.Wrap;
			BorderColor = ColorsF.White;
			MagFilter = TextureFilter.Bilinear;
			MinFilter = TextureFilter.Bilinear;
			MipFilter = TextureMipMapFilter.Linear;
		}
	}
}