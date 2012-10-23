using SlimShader.IO;
using SlimShader.Shader;

namespace SlimShader.ResourceDefinition
{
	/// <summary>
	/// Describes a shader-variable type.
	/// Based on D3D11_SHADER_TYPE_DESC.
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

		public string BaseTypeName { get; private set; }

		public static ShaderType Parse(BytecodeReader reader, BytecodeReader typeReader, ShaderVersion target)
		{
			var result = new ShaderType
			{
				VariableClass = (ShaderVariableClass) typeReader.ReadUInt16(),
				VariableType = (ShaderVariableType) typeReader.ReadUInt16(),
				Rows = typeReader.ReadUInt16(),
				Columns = typeReader.ReadUInt16(),
				ElementCount = typeReader.ReadUInt16(),
				MemberCount = typeReader.ReadUInt16(),
				MemberOffset = typeReader.ReadUInt32()
			};

			if (target.MajorVersion >= 5)
			{
				var parentTypeOffset = typeReader.ReadUInt32(); // Guessing
				var parentTypeReader = reader.CopyAtOffset((int) parentTypeOffset);
				var parentTypeClass = (ShaderVariableClass) parentTypeReader.ReadUInt16();
				var unknown4 = parentTypeReader.ReadUInt16();

				var unknown1 = typeReader.ReadUInt32();
				var unknown2 = typeReader.ReadUInt32();
				var unknown3 = typeReader.ReadUInt32();

				var parentNameOffset = typeReader.ReadUInt32();
				var parentNameReader = reader.CopyAtOffset((int) parentNameOffset);
				result.BaseTypeName = parentNameReader.ReadString();
			}

			// TODO: Parse members, see Wine for reference:
			// https://github.com/mirrors/wine/blob/master/dlls/d3dcompiler_43/reflection.c#L1235

			return result;
		}
	}
}