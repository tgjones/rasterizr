using System.Collections.Generic;
using System.Text;
using SlimShader.ObjectModel.Tokens;

namespace SlimShader.ObjectModel
{
	public abstract class ChunkContent
	{
		
	}

	public class ShaderProgram : ChunkContent
	{
		public ShaderVersion Version { get; internal set; }
		public uint Length { get; internal set; }
		public List<OpcodeToken> Tokens { get; private set; }

		public ShaderProgram()
		{
			Tokens = new List<OpcodeToken>();
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendLine(string.Format("{0}_{1}_{2}",
				Version.ProgramType.GetDescription(),
				Version.MajorVersion,
				Version.MinorVersion));

			foreach (var token in Tokens)
				sb.AppendLine(token.ToString());

			return sb.ToString();
		}
	}

	public class ResourceDefinition : ChunkContent
	{
		public List<ConstantBuffer> ConstantBuffers { get; private set; }

		public ResourceDefinition()
		{
			ConstantBuffers = new List<ConstantBuffer>();
		}
	}

	public class ConstantBuffer
	{
		public string Name { get; set; }
	}
}