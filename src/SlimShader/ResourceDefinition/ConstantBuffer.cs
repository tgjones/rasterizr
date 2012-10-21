using System.Collections.Generic;
using SlimShader.IO;

namespace SlimShader.ResourceDefinition
{
	public class ConstantBuffer
	{
		public string Name { get; private set; }
		public List<ConstantBufferVariable> Variables { get; private set; }
		public ShaderCBufferFlags Flags { get; private set; }

		public ConstantBuffer()
		{
			Variables = new List<ConstantBufferVariable>();
		}

		public static ConstantBuffer Parse(BytecodeReader reader, BytecodeReader resourceDefinitionReader)
		{
			uint nameOffset = reader.ReadUInt32();
			var nameReader = resourceDefinitionReader.CopyAtOffset((int) nameOffset);

			uint variableCount = reader.ReadUInt32();
			uint variablesOffset = reader.ReadUInt32();

			var result = new ConstantBuffer
			{
				Name = nameReader.ReadString()
			};

			var variablesReader = resourceDefinitionReader.CopyAtOffset((int) variablesOffset);
			for (int i = 0; i < variableCount; i++)
				result.Variables.Add(ConstantBufferVariable.Parse(variablesReader));

			uint size = reader.ReadUInt32();


			return result;
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