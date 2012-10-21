using System.ComponentModel;

namespace SlimShader.ObjectModel
{
	public enum ShaderInputType
	{
		[Description("cbuffer")]
		CBuffer = 0,

		TBuffer = 1,

		[Description("texture")]
		Texture = 2,

		[Description("sampler")]
		Sampler = 3,

		UavRwTyped = 4,
		Structured = 5,
		UavRwStructured = 6,
		ByteAddress = 7,
		UavRwByteAddress = 8,
		UavAppendStructured = 9,
		UavConsumeStructured = 10,
		UavRwStructuredWithCounter = 11
	}
}