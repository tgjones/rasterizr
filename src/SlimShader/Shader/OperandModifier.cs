using System.ComponentModel;

namespace SlimShader.Shader
{
	public enum OperandModifier
	{
		[Description("")]
		None = 0,

		[Description("-")]
		Neg = 1,

		Abs = 2,
		AbsNeg = 3,
	}
}