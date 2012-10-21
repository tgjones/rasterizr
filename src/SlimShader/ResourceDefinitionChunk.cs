using System.Collections.Generic;
using System.Text;
using SlimShader.IO;

namespace SlimShader
{
	/// <summary>
	/// Most of this was adapted from 
	/// https://devel.nuclex.org/framework/browser/graphics/Nuclex.Graphics.Native/trunk/Source/Introspection/HlslShaderReflector.cpp?rev=1743
	/// </summary>
	public class ResourceDefinitionChunk : DxbcChunk
	{
		public List<ConstantBuffer> ConstantBuffers { get; private set; }
		public List<ResourceBinding> ResourceBindings { get; private set; }

		public ResourceDefinitionChunk()
		{
			ConstantBuffers = new List<ConstantBuffer>();
			ResourceBindings = new List<ResourceBinding>();
		}

		public static ResourceDefinitionChunk Parse(BytecodeReader reader)
		{
			var startOfResourceDefinitionReader = reader.CopyAtCurrentPosition();
			var headerReader = reader.CopyAtCurrentPosition();

			uint constantBufferCount = headerReader.ReadUInt32();
			uint constantBufferOffset = headerReader.ReadUInt32();
			uint resourceBindingCount = headerReader.ReadUInt32();
			uint resourceBindingOffset = headerReader.ReadUInt32();
			uint requiredShaderVersion = headerReader.ReadUInt32();
			uint flags = headerReader.ReadUInt32();

			var result = new ResourceDefinitionChunk();

			var constantBufferReader = reader.CopyAtOffset((int) constantBufferOffset);
			for (int i = 0; i < constantBufferCount; i++)
				result.ConstantBuffers.Add(ConstantBuffer.Parse(constantBufferReader));

			var resourceBindingReader = reader.CopyAtOffset((int) resourceBindingOffset);
			for (int i = 0; i < resourceBindingCount; i++)
				result.ResourceBindings.Add(ResourceBinding.Parse(resourceBindingReader,
					startOfResourceDefinitionReader));

			return result;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendLine("// Buffer Definitions: ");
			sb.AppendLine("//");

			foreach (var constantBuffer in ConstantBuffers)
				sb.Append(constantBuffer);

			sb.AppendLine("//");
			sb.AppendLine("//");
			sb.AppendLine("// Resource Bindings:");
			sb.AppendLine("//");
			sb.AppendLine("// Name                                 Type  Format         Dim Slot Elements");
			sb.AppendLine("// ------------------------------ ---------- ------- ----------- ---- --------");

			foreach (var resourceBinding in ResourceBindings)
				sb.AppendLine(resourceBinding.ToString());

			return sb.ToString();
		}
	}
}