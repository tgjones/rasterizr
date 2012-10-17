namespace SlimShader.ObjectModel
{
	public class ShaderVersion
	{
		public byte MajorVersion { get; set; }
		public byte MinorVersion { get; set; }
		public ProgramType ProgramType { get; set; }
		public uint Length { get; set; }
	}
}