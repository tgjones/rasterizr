namespace Rasterizr.ShaderCore
{
	/// <summary>
	/// Maps the output of the previous shader stage into the input
	/// of the next shader stage.
	/// </summary>
	public class ShaderMapper
	{
		public void Map(object upstreamInstance, ShaderDescription upstreamDescription,
			object downstreamInstance, ShaderDescription downstreamDescription)
		{
			foreach (var inputParameter in downstreamDescription.InputParameters)
			{
				if (inputParameter.SystemValueType == SystemValueType.None)
				{
					var outputParameter = upstreamDescription.GetOutputParameterBySemantic(inputParameter.Semantic);
					var outputParameterValue = outputParameter.GetValue(upstreamInstance);
					inputParameter.SetValue(ref downstreamInstance, outputParameterValue);
				}
				else
				{
					// TODO: Set system value.
				}
			}
		}
	}
}