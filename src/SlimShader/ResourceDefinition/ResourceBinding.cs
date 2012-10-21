using SlimShader.IO;
using SlimShader.Shader;

namespace SlimShader.ResourceDefinition
{
	/// <summary>
	/// Roughly corresponds to the D3D11_SHADER_INPUT_BIND_DESC structure.
	/// </summary>
	public class ResourceBinding
	{
		public string Name { get; private set; }
		public ShaderInputType Type { get; private set; }
		public uint BindPoint { get; private set; }
		public uint BindCount { get; private set; }
		public ShaderInputFlags Flags { get; private set; }
		public ResourceDefinitionResourceDimension Dimension { get; private set; }
		public ResourceReturnType ReturnType { get; private set; }
		public uint NumSamples { get; private set; }

		public static ResourceBinding Parse(BytecodeReader reader, BytecodeReader resourceDefinitionReader)
		{
			uint nameOffset = reader.ReadUInt32();
			var nameReader = resourceDefinitionReader.CopyAtOffset((int) nameOffset);
			return new ResourceBinding
			{
				Name = nameReader.ReadString(),
				Type = (ShaderInputType) reader.ReadUInt32(),
				ReturnType = (ResourceReturnType) reader.ReadUInt32(),
				Dimension = (ResourceDefinitionResourceDimension) reader.ReadUInt32(),
				NumSamples = reader.ReadUInt32(),
				BindPoint = reader.ReadUInt32(),
				BindCount = reader.ReadUInt32(),
				Flags = (ShaderInputFlags) reader.ReadUInt32()
			};
		}

		public override string ToString()
		{
			string returnType = (ReturnType == ResourceReturnType.NotApplicable)
				? "NA" : ReturnType.GetDescription() + "4";
			return string.Format("// {0,-30} {1,10} {2,7} {3,11} {4,4} {5,8}",
				Name, Type.GetDescription(), returnType,
				Dimension.GetDescription() + (Dimension.IsMultiSampled() ? NumSamples.ToString() : string.Empty),
				BindPoint, BindCount);
		}
	}
}