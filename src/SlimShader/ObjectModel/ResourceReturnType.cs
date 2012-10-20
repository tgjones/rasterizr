using System.ComponentModel;

namespace SlimShader.ObjectModel
{
	public enum ResourceReturnType
	{
		[Description("unorm")]
		UNorm = 1,

		[Description("snorm")]
		SNorm = 2,

		[Description("sint")]
		SInt = 3,

		[Description("uint")]
		UInt = 4,

		[Description("float")]
		Float = 5,

		Mixed = 6,
		Double = 7,
		Continued = 8,
		Unused = 9
	}
}