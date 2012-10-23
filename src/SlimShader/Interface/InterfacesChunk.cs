using System.Collections.Generic;
using System.Text;
using SlimShader.IO;

namespace SlimShader.Interface
{
	public class InterfacesChunk : DxbcChunk
	{
		public List<ClassType> AvailableClassTypes { get; private set; }

		public InterfacesChunk()
		{
			AvailableClassTypes = new List<ClassType>();
		}

		public static InterfacesChunk Parse(BytecodeReader reader, uint sizeInBytes)
		{
			var headerReader = reader.CopyAtCurrentPosition();

			var unknown1 = headerReader.ReadUInt32();

			var classTypeCount = headerReader.ReadUInt32();
			var interfaceSlotCount = headerReader.ReadUInt32();

			var unknown4 = headerReader.ReadUInt32();

			var interfaceSlotOffset = headerReader.ReadUInt32();
			var interfaceSlotReader = reader.CopyAtOffset((int) interfaceSlotOffset);

			var classTypeOffset = headerReader.ReadUInt32();
			var classTypeReader = reader.CopyAtOffset((int) classTypeOffset);

			var result = new InterfacesChunk();

			for (uint i = 0; i < classTypeCount; i++)
			{
				var classType = ClassType.Parse(reader, classTypeReader);
				classType.ID = i; // Really??
				result.AvailableClassTypes.Add(classType);
			}

			for (int i = 0; i < interfaceSlotCount; i++)
			{
				var unknown13 = interfaceSlotReader.ReadUInt32();
				var unknown14 = interfaceSlotReader.ReadUInt32();

				var type0 = interfaceSlotReader.ReadUInt32();
				var type1 = interfaceSlotReader.ReadUInt32();
				var type2 = interfaceSlotReader.ReadUInt32();
			}

			return result;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendLine("//");
			sb.AppendLine("// Available Class Types:");
			sb.AppendLine("//");
			sb.AppendLine("// Name                             ID CB Stride Texture Sampler");
			sb.AppendLine("// ------------------------------ ---- --------- ------- -------");

			foreach (var classType in AvailableClassTypes)
				sb.AppendLine("// " + classType);

			sb.AppendLine("//");
			sb.AppendLine("// Interface slots, 3 total:");
			sb.AppendLine("//");
			sb.AppendLine("//             Slots");
			sb.AppendLine("// +----------+---------+---------------------------------------");
			sb.AppendLine("// | Type ID  |   0     |0    1    2    ");
			sb.AppendLine("// | Table ID |         |0    1    2    ");
			sb.AppendLine("// +----------+---------+---------------------------------------");
			sb.AppendLine("// | Type ID  |   1     |0    1    2    ");
			sb.AppendLine("// | Table ID |         |3    4    5    ");
			sb.AppendLine("// +----------+---------+---------------------------------------");
			sb.AppendLine("// | Type ID  |   2     |3    4    ");
			sb.AppendLine("// | Table ID |         |6    7    ");
			sb.AppendLine("// +----------+---------+---------------------------------------");

			return sb.ToString();
		}
	}
}