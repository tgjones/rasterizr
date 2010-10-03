using Nexus;

namespace Rasterizr.PipelineStages.OutputMerger
{
	public class BlendState
	{
		public static BlendState AlphaBlend
		{
			get
			{
				return new BlendState
				{
					BlendEnable = true,
					ColorSourceBlend = Blend.SourceAlpha,
					ColorDestinationBlend = Blend.InverseSourceAlpha
				};
			}
		}

		public bool BlendEnable { get; set; }
		public Blend ColorSourceBlend { get; set; }
		public Blend ColorDestinationBlend { get; set; }
		public BlendFunction ColorBlendFunction { get; set; }
		public ColorF BlendFactor { get; set; }

		public BlendState()
		{
			BlendEnable = false;
			ColorSourceBlend = Blend.One;
			ColorDestinationBlend = Blend.Zero;
			ColorBlendFunction = BlendFunction.Add;
			BlendFactor = ColorsF.White;
		}
	}
}