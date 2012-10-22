namespace SlimShader
{
	public enum ChunkType
	{
		Unknown,

		/// <summary>
		/// Input signature
		/// </summary>
		Isgn,

		/// <summary>
		/// Output signature (SM5)
		/// </summary>
		Osg5,

		/// <summary>
		/// Output signature
		/// </summary>
		Osgn,

		/// <summary>
		/// Resource definition
		/// </summary>
		Rdef,

		/// <summary>
		/// Shader
		/// </summary>
		Shdr,

		/// <summary>
		/// Shader
		/// </summary>
		Shex,

		/// <summary>
		/// ?
		/// </summary>
		Stat
	}
}