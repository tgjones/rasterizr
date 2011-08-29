namespace Rasterizr
{
	public enum ComparisonFunc
	{
		/// <summary>
		/// Never pass the comparison.
		/// </summary>
		Never,

		/// <summary>
		/// If the source data is less than the destination data, the comparison passes.
		/// </summary>
		Less,

		/// <summary>
		/// If the source data is equal to the destination data, the comparison passes.
		/// </summary>
		Equal,

		/// <summary>
		/// If the source data is less than or equal to the destination data, the comparison passes.
		/// </summary>
		LessEqual,

		/// <summary>
		/// If the source data is greater than the destination data, the comparison passes.
		/// </summary>
		Greater,

		/// <summary>
		/// If the source data is not equal to the destination data, the comparison passes.
		/// </summary>
		NotEqual,

		/// <summary>
		/// If the source data is greater than or equal to the destination data, the comparison passes.
		/// </summary>
		GreaterEqual,

		/// <summary>
		/// Always pass the comparison.
		/// </summary>
		Always
	}
}