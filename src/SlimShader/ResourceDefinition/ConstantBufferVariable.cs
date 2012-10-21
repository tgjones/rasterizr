using SlimShader.IO;

namespace SlimShader.ResourceDefinition
{
	public class ConstantBufferVariable
	{
		/// <summary>
		/// The variable name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Offset from the start of the parent structure, to the beginning of the variable.
		/// </summary>
		public uint StartOffset { get; private set; }

		/// <summary>
		/// Size of the variable (in bytes).
		/// </summary>
		public uint Size { get; set; }

		/// <summary>
		/// Flags, which identify shader-variable properties.
		/// </summary>
		public ShaderVariableFlags Flags { get; private set; }

		public ShaderType ShaderType { get; private set; }

		/// <summary>
		/// The default value for initializing the variable.
		/// </summary>
		public object DefaultValue { get; private set; }

		public static ConstantBufferVariable Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			uint nameOffset = variableReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int) nameOffset);

			var startOffset = variableReader.ReadUInt32();
			uint size = variableReader.ReadUInt32();
			var flags = (ShaderVariableFlags) variableReader.ReadUInt32();

			var typeOffset = variableReader.ReadUInt32();
			var typeReader = reader.CopyAtOffset((int) typeOffset);
			var shaderType = ShaderType.Parse(reader, typeReader);

			var defaultValueOffset = variableReader.ReadUInt32();
			if (defaultValueOffset != 0)
			{
				var defaultValueReader = reader.CopyAtOffset((int) defaultValueOffset);
				// TODO: Read default value
			}

			return new ConstantBufferVariable
			{
				Name = nameReader.ReadString(),
				StartOffset = startOffset,
				Size = size,
				Flags = flags,
				ShaderType = shaderType
			};
		}

		public override string ToString()
		{
			// For example:
			// float4 cool;                       // Offset:    0 Size:    16

			string variableType = ShaderType.VariableType.GetDescription();
			variableType += ShaderType.Columns;

			string arrayCount = string.Empty;
			if (ShaderType.ElementCount > 0)
				arrayCount = "[" + ShaderType.ElementCount + "]";

			string declaration = string.Format("{0} {1}{2};", variableType, Name, arrayCount);
			return string.Format("{0,-34} // Offset: {1,4} Size: {2,5}", declaration, StartOffset, Size);
		}
	}
}