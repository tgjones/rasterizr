using System.Collections.Generic;

namespace Rasterizr.Toolkit.Effects
{
	public class Effect
	{
		private EffectTechnique _currentTechnique;

		public DeviceContext DeviceContext { get; private set; }
		public List<EffectTechnique> Techniques { get; set; }

		public EffectTechnique CurrentTechnique
		{
			get { return _currentTechnique ?? (_currentTechnique = Techniques[0]); }
			set { _currentTechnique = value; }
		}

		public Effect(DeviceContext deviceContext)
		{
			DeviceContext = deviceContext;
			Techniques = new List<EffectTechnique>();
		}

		protected internal virtual void OnApply()
		{
			
		}
	}
}