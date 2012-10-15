using System.Collections.Generic;

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
}