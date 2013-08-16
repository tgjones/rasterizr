using System;
using Rasterizr.Util;

namespace Rasterizr.Resources
{
	public class SamplerState : DeviceChild
	{
		private readonly SamplerStateDescription _description;
        private readonly SlimShader.VirtualMachine.Resources.SamplerState _virtualMachineSamplerState;

		public SamplerStateDescription Description
		{
			get { return _description; }
		}

        internal SlimShader.VirtualMachine.Resources.SamplerState VirtualMachineSamplerState
		{
            get { return _virtualMachineSamplerState; }
		}

		internal SamplerState(Device device, SamplerStateDescription description)
			: base(device)
		{
			_description = description;
		    _virtualMachineSamplerState = new SlimShader.VirtualMachine.Resources.SamplerState
		    {
		        Filter = ConvertFilter(description.Filter),
                AddressU = ConvertAddress(description.AddressU),
                AddressV = ConvertAddress(description.AddressV),
                AddressW = ConvertAddress(description.AddressW),
                MinimumLod = description.MinimumLod,
                MaximumLod = description.MaximumLod,
                MipLodBias = description.MipLodBias,
                MaximumAnisotropy = description.MaximumAnisotropy,
                ComparisonFunction = ConvertComparison(description.ComparisonFunction),
                BorderColor = description.BorderColor.ToNumber4()
            };
		}

	    private static SlimShader.VirtualMachine.Resources.Filter ConvertFilter(Filter filter)
	    {
	        switch (filter)
	        {
	            case Filter.MinMagMipPoint:
	                return SlimShader.VirtualMachine.Resources.Filter.MinMagMipPoint;
	            case Filter.MinMagPointMipLinear:
	                return SlimShader.VirtualMachine.Resources.Filter.MinMagPointMipLinear;
	            case Filter.MinPointMagLinearMipPoint:
	                return SlimShader.VirtualMachine.Resources.Filter.MinPointMagLinearMipPoint;
	            case Filter.MinPointMagMipLinear:
	                return SlimShader.VirtualMachine.Resources.Filter.MinPointMagMipLinear;
	            case Filter.MinLinearMagMipPoint:
	                return SlimShader.VirtualMachine.Resources.Filter.MinLinearMagMipPoint;
	            case Filter.MinLinearMagPointMipLinear:
	                return SlimShader.VirtualMachine.Resources.Filter.MinLinearMagPointMipLinear;
	            case Filter.MinMagLinearMipPoint:
	                return SlimShader.VirtualMachine.Resources.Filter.MinMagLinearMipPoint;
	            case Filter.MinMagMipLinear:
	                return SlimShader.VirtualMachine.Resources.Filter.MinMagMipLinear;
	            case Filter.Anisotropic:
	                return SlimShader.VirtualMachine.Resources.Filter.Anisotropic;
	            case Filter.ComparisonMinMagMipPoint:
	                return SlimShader.VirtualMachine.Resources.Filter.ComparisonMinMagMipPoint;
	            case Filter.ComparisonMinMagPointMipLinear:
	                return SlimShader.VirtualMachine.Resources.Filter.ComparisonMinMagPointMipLinear;
	            case Filter.ComparisonMinPointMagLinearMipPoint:
	                return SlimShader.VirtualMachine.Resources.Filter.ComparisonMinPointMagLinearMipPoint;
	            case Filter.ComparisonMinPointMagMipLinear:
	                return SlimShader.VirtualMachine.Resources.Filter.ComparisonMinPointMagMipLinear;
	            case Filter.ComparisonMinLinearMagMipPoint:
	                return SlimShader.VirtualMachine.Resources.Filter.ComparisonMinLinearMagMipPoint;
	            case Filter.ComparisonMinLinearMagPointMipLinear:
	                return SlimShader.VirtualMachine.Resources.Filter.ComparisonMinLinearMagPointMipLinear;
	            case Filter.ComparisonMinMagLinearMipPoint:
	                return SlimShader.VirtualMachine.Resources.Filter.ComparisonMinMagLinearMipPoint;
	            case Filter.ComparisonMinMagMipLinear:
	                return SlimShader.VirtualMachine.Resources.Filter.ComparisonMinMagMipLinear;
	            case Filter.ComparisonAnisotropic:
	                return SlimShader.VirtualMachine.Resources.Filter.ComparisonAnisotropic;
	            default:
	                throw new ArgumentOutOfRangeException("filter");
	        }
	    }

	    private static SlimShader.VirtualMachine.Resources.TextureAddressMode ConvertAddress(TextureAddressMode address)
	    {
	        switch (address)
	        {
	            case TextureAddressMode.Wrap:
	                return SlimShader.VirtualMachine.Resources.TextureAddressMode.Wrap;
	            case TextureAddressMode.Mirror:
	                return SlimShader.VirtualMachine.Resources.TextureAddressMode.Mirror;
	            case TextureAddressMode.Clamp:
	                return SlimShader.VirtualMachine.Resources.TextureAddressMode.Clamp;
	            case TextureAddressMode.Border:
	                return SlimShader.VirtualMachine.Resources.TextureAddressMode.Border;
	            case TextureAddressMode.MirrorOnce:
	                return SlimShader.VirtualMachine.Resources.TextureAddressMode.MirrorOnce;
	            default:
	                throw new ArgumentOutOfRangeException("address");
	        }
	    }

	    private static SlimShader.VirtualMachine.Resources.Comparison ConvertComparison(Comparison comparison)
	    {
	        switch (comparison)
	        {
	            case Comparison.Never:
	                return SlimShader.VirtualMachine.Resources.Comparison.Never;
	            case Comparison.Less:
	                return SlimShader.VirtualMachine.Resources.Comparison.Less;
	            case Comparison.Equal:
	                return SlimShader.VirtualMachine.Resources.Comparison.Equal;
	            case Comparison.LessEqual:
	                return SlimShader.VirtualMachine.Resources.Comparison.LessEqual;
	            case Comparison.Greater:
	                return SlimShader.VirtualMachine.Resources.Comparison.Greater;
	            case Comparison.NotEqual:
	                return SlimShader.VirtualMachine.Resources.Comparison.NotEqual;
	            case Comparison.GreaterEqual:
	                return SlimShader.VirtualMachine.Resources.Comparison.GreaterEqual;
	            case Comparison.Always:
	                return SlimShader.VirtualMachine.Resources.Comparison.Always;
	            default:
	                throw new ArgumentOutOfRangeException("comparison");
	        }
	    }
	}
}