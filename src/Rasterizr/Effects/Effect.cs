using System.Collections.Generic;
using Rasterizr.Core;

namespace Rasterizr.Effects
{
	public class Effect
	{
		private EffectTechnique _currentTechnique;

		public RasterizrDevice Device { get; private set; }
		public List<EffectTechnique> Techniques { get; set; }

		public EffectTechnique CurrentTechnique
		{
			get { return _currentTechnique ?? (_currentTechnique = Techniques[0]); }
			set { _currentTechnique = value; }
		}

		public Effect(RasterizrDevice device)
		{
			Device = device;
			Techniques = new List<EffectTechnique>();
		}

		protected internal virtual void OnApply()
		{
			
		}
	}
}