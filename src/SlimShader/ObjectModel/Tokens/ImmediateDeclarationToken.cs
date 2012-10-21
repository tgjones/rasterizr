namespace SlimShader.ObjectModel.Tokens
{
	public abstract class ImmediateDeclarationToken : CustomDataToken
	{
		public uint DeclarationLength { get; internal set; }
	}
}