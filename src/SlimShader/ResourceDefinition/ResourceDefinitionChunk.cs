using System;
using System.Collections.Generic;
using System.Text;
using SlimShader.IO;
using SlimShader.Shader;
using SlimShader.Util;

namespace SlimShader.ResourceDefinition
{
	public enum ShaderFlags
	{
		None = 0,
		Debug = 1,
		SkipValidation = 2,
		SkipOptimization = 4,
		PackMatrixRowMajor = 8,
		PackMatrixColumnMajor = 16,
		PartialPrecision = 32,
		ForceVsSoftwareNoOpt = 64,
		ForcePsSoftwareNoOpt = 128,
		NoPreshader = 256,
		AvoidFlowControl = 512,
		PreferFlowControl = 1024,
		EnableStrictness = 2048,
		EnableBackwardsCompatibility = 4096,
		IeeeStrictness = 8192,
		OptimizationLevel0 = 16384,
		OptimizationLevel1 = 0,
		OptimizationLevel2 = 49152,
		OptimizationLevel3 = 32768,
		Reserved16 = 65536,
		Reserved17 = 131072,
		WarningsAreErrors = 262144
	}

	/// <summary>
	/// Most of this was adapted from 
	/// https://devel.nuclex.org/framework/browser/graphics/Nuclex.Graphics.Native/trunk/Source/Introspection/HlslShaderReflector.cpp?rev=1743
	/// Roughly corresponds to the D3D11_SHADER_DESC structure.
	/// </summary>
	public class ResourceDefinitionChunk : DxbcChunk
	{
		public List<ConstantBuffer> ConstantBuffers { get; private set; }
		public List<ResourceBinding> ResourceBindings { get; private set; }
		public ShaderVersion Target { get; private set; }
		public ShaderFlags Flags { get; private set; }
		public string Creator { get; private set; }

		public ResourceDefinitionChunk()
		{
			ConstantBuffers = new List<ConstantBuffer>();
			ResourceBindings = new List<ResourceBinding>();
		}

		public static ResourceDefinitionChunk Parse(BytecodeReader reader)
		{
			var headerReader = reader.CopyAtCurrentPosition();

			uint constantBufferCount = headerReader.ReadUInt32();
			uint constantBufferOffset = headerReader.ReadUInt32();
			uint resourceBindingCount = headerReader.ReadUInt32();
			uint resourceBindingOffset = headerReader.ReadUInt32();
			uint target = headerReader.ReadUInt32();
			uint flags = headerReader.ReadUInt32();

			var creatorOffset = headerReader.ReadUInt32();
			var creatorReader = reader.CopyAtOffset((int) creatorOffset);
			var creator = creatorReader.ReadString();

			// TODO: Maybe move this into a ShaderTarget class.
			ProgramType programType;
			switch (target.DecodeValue<ushort>(16, 31))
			{
				case 0xFFFF :
					programType = ProgramType.PixelShader;
					break;
				default :
					throw new ArgumentOutOfRangeException();
			}

			var result = new ResourceDefinitionChunk
			{
				Target = new ShaderVersion
				{
					MajorVersion = target.DecodeValue<byte>(8, 15),
					MinorVersion = target.DecodeValue<byte>(0, 7),
					ProgramType = programType
				},
				Flags = (ShaderFlags) flags,
				Creator = creator
			};

			var constantBufferReader = reader.CopyAtOffset((int) constantBufferOffset);
			for (int i = 0; i < constantBufferCount; i++)
				result.ConstantBuffers.Add(ConstantBuffer.Parse(reader, constantBufferReader));

			var resourceBindingReader = reader.CopyAtOffset((int) resourceBindingOffset);
			for (int i = 0; i < resourceBindingCount; i++)
				result.ResourceBindings.Add(ResourceBinding.Parse(reader, resourceBindingReader));

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