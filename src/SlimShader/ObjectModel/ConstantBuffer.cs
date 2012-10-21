using SlimShader.IO;

namespace SlimShader.ObjectModel
{
	public class ConstantBuffer
	{
		public string Name { get; set; }

		public static ConstantBuffer Parse(BytecodeReader reader)
		{
			return new ConstantBuffer();
		}
	}
}