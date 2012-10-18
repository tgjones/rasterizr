using System.Collections.Generic;
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