namespace SlimShader.ObjectModel
{
	public class OpcodeHeader
	{
		public Opcode Opcode { get; set; }
		public uint Length { get; set; }
		public bool IsExtended { get; set; }
	}
}