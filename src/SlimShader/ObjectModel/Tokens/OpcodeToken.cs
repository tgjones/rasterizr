namespace SlimShader.ObjectModel.Tokens
{
	public abstract class OpcodeToken
	{
		public OpcodeHeader Header { get; internal set; }

		protected string TypeDescription
		{
			get { return Header.OpcodeType.GetDescription(); }
		}

		public override string ToString()
		{
			return TypeDescription;
		}
	}

	public abstract class DeclarationToken : OpcodeToken
	{
		public Operand Operand { get; internal set; }
	}

	public class IndexingRangeDeclarationToken : DeclarationToken
	{
		/// <summary>
		/// Represents the count of registers starting the from the one indicated in Operand.
		/// </summary>
		public uint RegisterCount { get; internal set; }
	}

	public class TempRegisterDeclarationToken : DeclarationToken
	{
		/// <summary>
		/// Indicates how many temps are being declared. i.e. 5 means r0...r4 are declared.
		/// </summary>
		public uint TempCount { get; internal set; }

		public override string ToString()
		{
			return string.Format("dcl_temps {0}", TempCount);
		}
	}

	public class IndexableTempRegisterDeclarationToken : DeclarationToken
	{
		/// <summary>
		/// Register index (defines which x# register is declared)
		/// </summary>
		public uint RegisterIndex { get; internal set; }

		/// <summary>
		/// Number of registers in this register bank
		/// </summary>
		public uint RegisterCount { get; internal set; }

		/// <summary>
		/// Number of components in the array (1-4). 1 means .x, 2 means .xy, etc.
		/// </summary>
		public uint NumComponents { get; internal set; }

		public override string ToString()
		{
			return string.Format("{0} x{1}[{2}], {3}", TypeDescription,
				RegisterIndex, RegisterCount, NumComponents);
		}
	}

	public abstract class ImmediateDeclarationToken : DeclarationToken
	{
		public uint DeclarationLength { get; internal set; }
	}

	public class ImmediateConstantBufferDeclarationToken : ImmediateDeclarationToken
	{
		public uint[] Data { get; internal set; }
	}

	public class ShaderMessageDeclarationToken : ImmediateDeclarationToken
	{
		/// <summary>
		/// Indicates the info queue message ID.
		/// </summary>
		public uint InfoQueueMessageID { get; internal set; }

		/// <summary>
		/// Indicates the convention for formatting the message.
		/// </summary>
		public ShaderMessageFormat MessageFormat { get; internal set; }

		/// <summary>
		/// DWORD indicating the number of characters in the string without the terminator.
		/// </summary>
		public uint NumCharacters { get; internal set; }

		/// <summary>
		/// DWORD indicating the number of operands.
		/// </summary>
		public uint NumOperands { get; internal set; }

		/// <summary>
		/// DWORD indicating length of operands.
		/// </summary>
		public uint OperandsLength { get; internal set; }

		// Not sure what format this is.
		public object EncodedOperands { get; internal set; }

		/// <summary>
		/// String with trailing zero, padded to a multiple of DWORDs.
		/// The string is in the given format and the operands given should
		/// be used for argument substitutions when formatting.
		/// </summary>
		public string Format { get; internal set; }
	}

	public class GeometryShaderInputPrimitiveDeclarationToken : DeclarationToken
	{
		public Primitive Primitive { get; set; }
	}

	public class GeometryShaderOutputPrimitiveTopologyDeclarationToken : DeclarationToken
	{
		public PrimitiveTopology PrimitiveTopology { get; set; }
	}

	public class GeometryShaderMaxOutputVertexCountDeclarationToken : DeclarationToken
	{
		public uint MaxPrimitives { get; set; }
	}

	public class GeometryShaderInstanceCountDeclarationToken : DeclarationToken
	{
		public uint InstanceCount { get; set; }
	}

	public class ControlPointCountDeclarationToken : DeclarationToken
	{
		public uint ControlPointCount { get; set; }
	}

	public class TessellatorDomainDeclarationToken : DeclarationToken
	{
		public TessellatorDomain Domain { get; set; }
	}

	public class TessellatorPartioningDeclarationToken : DeclarationToken
	{
		public TessellatorPartitioning Partioning { get; set; }
	}

	public class TessellatorOutputPrimitiveDeclarationToken : DeclarationToken
	{
		public TessellatorOutputPrimitive OutputPrimitive { get; set; }
	}

	public class HullShaderMaxTessFactorDeclarationToken : DeclarationToken
	{
		public float MaxTessFactor { get; set; }
	}

	public class HullShaderForkPhaseInstanceCountDeclarationToken : DeclarationToken
	{
		public uint InstanceCount { get; set; }
	}
}