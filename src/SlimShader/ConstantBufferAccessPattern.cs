using System.ComponentModel;

namespace SlimShader
{
	public enum ConstantBufferAccessPattern
	{
		[Description("immediateIndexed")]
		ImmediateIndexed = 0,

		[Description("dynamicIndexed")]
		DynamicIndexed = 1
	}
}