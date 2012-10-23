using System.Text;
using SlimShader.IO;

namespace SlimShader
{
	public class InterfacesChunk : DxbcChunk
	{
		public static InterfacesChunk Parse(BytecodeReader reader, uint sizeInBytes)
		{
			var headerReader = reader.CopyAtCurrentPosition();

			var unknown1 = headerReader.ReadUInt32();

			var unknown2 = headerReader.ReadUInt32();
			var interfaceSlotCount = headerReader.ReadUInt32();

			var unknown4 = headerReader.ReadUInt32();

			var interfaceSlotOffset = headerReader.ReadUInt32();
			var interfaceSlotReader = reader.CopyAtOffset((int) interfaceSlotOffset);

			var offset2 = headerReader.ReadUInt32();
			var classLinkageReader = reader.CopyAtOffset((int) offset2);

			for (int i = 0; i < unknown2; i++)
			{
				var nameOffset = classLinkageReader.ReadUInt32();
				var nameReader = reader.CopyAtOffset((int) nameOffset);
				var name = nameReader.ReadString();

				var unknown11 = classLinkageReader.ReadUInt32();
				var unknown12 = classLinkageReader.ReadUInt32();
			}

			for (int i = 0; i < interfaceSlotCount; i++)
			{
				var unknown13 = interfaceSlotReader.ReadUInt32();
				var unknown14 = interfaceSlotReader.ReadUInt32();

				var type0 = interfaceSlotReader.ReadUInt32();
				var type1 = interfaceSlotReader.ReadUInt32();
				var type2 = interfaceSlotReader.ReadUInt32();
			}

			return new InterfacesChunk();
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendLine("//");
			sb.AppendLine("// Available Class Types:");
			sb.AppendLine("//");
			sb.AppendLine("// Name                             ID CB Stride Texture Sampler");
			sb.AppendLine("// ------------------------------ ---- --------- ------- -------");
			sb.AppendLine("// cUnchangedColour                  0         0       0       0");
			sb.AppendLine("// cHalfColour                       1         0       0       0");
			sb.AppendLine("// cDoubleColour                     2         0       0       0");
			sb.AppendLine("// TwoThirdsAlpha                    3         0       0       0");
			sb.AppendLine("// OneAlpha                          4         0       0       0");
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

	public class ClassInstance
	{
		public string Name { get; set; }
	}
}