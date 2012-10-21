using SlimShader.IO;

namespace SlimShader.ResourceDefinition
{
	/// <summary>
	/// Describes a shader-variable type.
	/// </summary>
	public class ShaderType
	{
		/// <summary>
		/// Identifies the variable class as one of scalar, vector, matrix or object.
		/// </summary>
		public ShaderVariableClass VariableClass { get; private set; }

		/// <summary>
		/// The variable type.
		/// </summary>
		public ShaderVariableType VariableType { get; private set; }

		/// <summary>
		/// Number of rows in a matrix. Otherwise a numeric type returns 1, any other type returns 0.
		/// </summary>
		public ushort Rows { get; private set; }

		/// <summary>
		/// Number of columns in a matrix. Otherwise a numeric type returns 1, any other type returns 0.
		/// </summary>
		public ushort Columns { get; private set; }

		/// <summary>
		/// Number of elements in an array; otherwise 0.
		/// </summary>
		public ushort ElementCount { get; private set; }

		/// <summary>
		/// Number of members in the structure; otherwise 0.
		/// </summary>
		public ushort MemberCount { get; private set; }

		/// <summary>
		/// Offset, in bytes, between the start of the parent structure and this variable.
		/// </summary>
		public uint MemberOffset { get; private set; }

		public static ShaderType Parse(BytecodeReader reader, BytecodeReader typeReader)
		{
			return new ShaderType
			{
				VariableClass = (ShaderVariableClass) typeReader.ReadUInt16(),
				VariableType = (ShaderVariableType) typeReader.ReadUInt16(),
				Rows = typeReader.ReadUInt16(),
				Columns = typeReader.ReadUInt16(),
				ElementCount = typeReader.ReadUInt16(),
				MemberCount = typeReader.ReadUInt16(),
				MemberOffset = typeReader.ReadUInt32()
			};
		}
	}
}