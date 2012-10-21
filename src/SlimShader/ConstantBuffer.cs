using SlimShader.IO;

namespace SlimShader
{
	public class ConstantBuffer
	{
		public string Name { get; set; }

		public static ConstantBuffer Parse(BytecodeReader reader)
		{
			return new ConstantBuffer();
		}

		public override string ToString()
		{
			return @"// cbuffer cbuf0
// {
//
//   float4 cool;                       // Offset:    0 Size:    16
//   int4 zeek;                         // Offset:   16 Size:    16
//   int2 arr[127];                     // Offset:   32 Size:  2024
//
// }
";
		}
	}
}