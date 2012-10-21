using System.Collections.Generic;
using System.Text;
using SlimShader.IO;

namespace SlimShader.ResourceDefinition
{
	public class ConstantBuffer
	{
		public string Name { get; private set; }
		public List<ConstantBufferVariable> Variables { get; private set; }
		public uint Size { get; private set; }
		public ShaderCBufferFlags Flags { get; private set; }
		public CBufferType BufferType { get; private set; }

		public ConstantBuffer()
		{
			Variables = new List<ConstantBufferVariable>();
		}

		public static ConstantBuffer Parse(BytecodeReader reader, BytecodeReader constantBufferReader)
		{
			uint nameOffset = constantBufferReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int) nameOffset);

			uint variableCount = constantBufferReader.ReadUInt32();
			uint variableOffset = constantBufferReader.ReadUInt32();

			var result = new ConstantBuffer
			{
				Name = nameReader.ReadString()
			};

			var variableReader = reader.CopyAtOffset((int) variableOffset);
			for (int i = 0; i < variableCount; i++)
				result.Variables.Add(ConstantBufferVariable.Parse(reader, variableReader));

			result.Size = constantBufferReader.ReadUInt32();
			result.Flags = (ShaderCBufferFlags) constantBufferReader.ReadUInt32();
			result.BufferType = (CBufferType) constantBufferReader.ReadUInt32();

			return result;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine("// cbuffer " + Name);
			sb.AppendLine("// {");
			sb.AppendLine("//");

			foreach (var variable in Variables)
				sb.AppendLine("//   " + variable);

			sb.AppendLine("//");
			sb.AppendLine("// }");
			return sb.ToString();
		}
	}
}