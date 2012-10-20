using System.ComponentModel;

namespace SlimShader.ObjectModel
{
	public enum SamplerMode
	{
		[Description("mode_default")]
		Default = 0,

		[Description("comparison")]
		Comparison = 1,

		[Description("mono")]
		Mono = 2
	}
}