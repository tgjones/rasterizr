using SlimShader.VirtualMachine.Resources;

namespace Rasterizr.Resources
{
	public class SamplerState : DeviceChild
	{
		private class Sampler : ISamplerState
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
        private readonly ISamplerState _sampler;

		public SamplerStateDescription Description
		{
			get { return _description; }
		}

        internal ISamplerState InnerSampler
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