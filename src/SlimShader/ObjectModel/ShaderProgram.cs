using System.Collections.Generic;
using System.Linq;
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

			int indent = 0;
			foreach (var token in Tokens)
			{
				if (token.Header.OpcodeType == OpcodeType.EndLoop || token.Header.OpcodeType == OpcodeType.EndIf
					|| token.Header.OpcodeType == OpcodeType.Else)
					indent -= 2;
				sb.AppendLine(string.Join(string.Empty, Enumerable.Repeat(" ", indent)) + token);
				// TODO: Change this, and other checks on enum values such as IsDeclaration and IsIntegerOperation,
				// into extension methods.
				if (token.Header.OpcodeType == OpcodeType.Loop || token.Header.OpcodeType == OpcodeType.If
					|| token.Header.OpcodeType == OpcodeType.Else)
					indent += 2;
			}

			sb.AppendFormat("// Approximately {0} instruction slots used", Tokens.OfType<InstructionToken>().Count());

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