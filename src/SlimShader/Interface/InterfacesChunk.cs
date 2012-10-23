using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SlimShader.IO;

namespace SlimShader.Interface
{
	public class InterfacesChunk : DxbcChunk
	{
		public List<ClassType> AvailableClassTypes { get; private set; }
		public List<InterfaceSlot> InterfaceSlots { get; private set; }

		public InterfacesChunk()
		{
			AvailableClassTypes = new List<ClassType>();
			InterfaceSlots = new List<InterfaceSlot>();
		}

		public static InterfacesChunk Parse(BytecodeReader reader, uint sizeInBytes)
		{
			var headerReader = reader.CopyAtCurrentPosition();

			Debug.Assert(headerReader.ReadUInt32() == 0); // Unknown

			var classTypeCount = headerReader.ReadUInt32();
			var interfaceSlotCount = headerReader.ReadUInt32();

			Debug.Assert(headerReader.ReadUInt32() == interfaceSlotCount); // Unknown

			headerReader.ReadUInt32(); // Think this is offset to start of interface slot info, but we don't need it.

			var classTypeOffset = headerReader.ReadUInt32();
			var classTypeReader = reader.CopyAtOffset((int) classTypeOffset);

			var interfaceSlotOffset = headerReader.ReadUInt32();
			var interfaceSlotReader = reader.CopyAtOffset((int) interfaceSlotOffset);

			var result = new InterfacesChunk();

			for (uint i = 0; i < classTypeCount; i++)
			{
				var classType = ClassType.Parse(reader, classTypeReader);
				classType.ID = i; // Really??
				result.AvailableClassTypes.Add(classType);
			}

			for (uint i = 0; i < interfaceSlotCount; i++)
			{
				var interfaceSlot = InterfaceSlot.Parse(reader, interfaceSlotReader);
				interfaceSlot.ID = i; // Really??
				result.InterfaceSlots.Add(interfaceSlot);
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
			sb.AppendLine(string.Format("// Interface slots, {0} total:", InterfaceSlots.Count));
			sb.AppendLine("//");
			sb.AppendLine("//             Slots");
			sb.AppendLine("// +----------+---------+---------------------------------------");

			foreach (var interfaceSlot in InterfaceSlots)
				sb.Append(interfaceSlot);

			return sb.ToString();
		}
	}
}