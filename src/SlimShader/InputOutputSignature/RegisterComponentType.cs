using System.ComponentModel;

namespace SlimShader.InputOutputSignature
{
	public enum RegisterComponentType
	{
		Unknown = 0,
		UInt32 = 1,
		SInt32 = 2,

		[Description("float")]
		Float32 = 3
	}
}