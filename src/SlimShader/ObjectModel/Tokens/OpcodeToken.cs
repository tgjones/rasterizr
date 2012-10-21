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

	public abstract class ImmediateDeclarationToken : DeclarationToken
	{
		public uint DeclarationLength { get; internal set; }
	}
}