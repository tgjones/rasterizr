using Nexus;

namespace Rasterizr.Core.ShaderCore
{
	public class SamplerState
	{
		#region Static stuff

		public static readonly SamplerState LinearWrap;
		public static readonly SamplerState PointWrap;

		static SamplerState()
		{
			LinearWrap = new SamplerState
			{
				AddressU = TextureAddressMode.Wrap,
				AddressV = TextureAddressMode.Wrap,
				Filter = TextureFilter.MinMagMipLinear,
			};
			PointWrap = new SamplerState
			{
				AddressU = TextureAddressMode.Wrap,
				AddressV = TextureAddressMode.Wrap,
				Filter = TextureFilter.MinMagMipPoint,
			};
		}

		#endregion

		public TextureFilter Filter { get; set; }
		public TextureAddressMode AddressU { get; set; }
		public TextureAddressMode AddressV { get; set; }
		public ColorF BorderColor { get; set; }

		public SamplerState()
		{
			Filter = TextureFilter.MinMagMipPoint;
			AddressU = TextureAddressMode.Clamp;
			AddressV = TextureAddressMode.Clamp;
			BorderColor = ColorsF.Black;
		}
	}
}