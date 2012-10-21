namespace SlimShader.ResourceDefinition
{
	/// <summary>
	/// Indicates a constant buffer's type.
	/// Based on D3D11_CBUFFER_TYPE.
	/// </summary>
	public enum CBufferType
	{
		/// <summary>
		/// A buffer containing scalar constants.
		/// </summary>
		CBuffer,

		/// <summary>
		/// A buffer containing texture data.
		/// </summary>
		TBuffer,

		/// <summary>
		/// A buffer containing interface pointers.
		/// </summary>
		InterfacePointers,

		/// <summary>
		/// A buffer containing binding information.
		/// </summary>
		ResourceBindInfo
	}
}