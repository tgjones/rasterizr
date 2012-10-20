using System.ComponentModel;

namespace SlimShader.ObjectModel
{
	public enum ConstantBufferAccessPattern
	{
		[Description("immediateIndexed")]
		ImmediateIndexed = 0,

		[Description("dynamicIndexed")]
		DynamicIndexed = 1
	}
}