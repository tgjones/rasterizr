using System.Collections.Generic;

namespace Rasterizr.Effects
{
	public class EffectTechnique
	{
		internal Effect ParentEffect { get; set; }

		public List<EffectPass> Passes { get; set; }

		public EffectTechnique(Effect parentEffect)
		{
			ParentEffect = parentEffect;
			Passes = new List<EffectPass>();
		}
	}
}