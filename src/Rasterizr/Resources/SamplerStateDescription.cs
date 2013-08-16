namespace Rasterizr.Resources
{
	public struct SamplerStateDescription
	{
		public static SamplerStateDescription Default
		{
			get
			{
				return new SamplerStateDescription
				{
					Filter = Filter.MinMagMipLinear,
					AddressU = TextureAddressMode.Clamp,
					AddressV = TextureAddressMode.Clamp,
					AddressW = TextureAddressMode.Clamp,
					MinimumLod = -float.MaxValue,
					MaximumLod = float.MaxValue,
					MipLodBias = 0.0f,
					MaximumAnisotropy = 16,
					ComparisonFunction = Comparison.Never,
					BorderColor = Color4.White
				};
			}
		}

		public Filter Filter;
		public TextureAddressMode AddressU;
		public TextureAddressMode AddressV;
		public TextureAddressMode AddressW;
		public float MinimumLod;
		public float MaximumLod;
		public float MipLodBias;
		public int MaximumAnisotropy;
		public Comparison ComparisonFunction;
		public Color4 BorderColor;
	}
}