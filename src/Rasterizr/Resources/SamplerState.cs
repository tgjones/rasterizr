using SlimShader.VirtualMachine.Resources;

namespace Rasterizr.Resources
{
	public class SamplerState : DeviceChild
	{
		private class Sampler : ISampler
		{
			private readonly SamplerStateDescription _description;

			public object Description
			{
				get { return _description; }
			}

			public Sampler(SamplerStateDescription description)
			{
				_description = description;
			}
		}

		private readonly SamplerStateDescription _description;
		private readonly ISampler _sampler;

		public SamplerStateDescription Description
		{
			get { return _description; }
		}

		internal ISampler InnerSampler
		{
			get { return _sampler; }
		}

		internal SamplerState(Device device, SamplerStateDescription description)
			: base(device)
		{
			_description = description;
			_sampler = new Sampler(description);
		}
	}
}