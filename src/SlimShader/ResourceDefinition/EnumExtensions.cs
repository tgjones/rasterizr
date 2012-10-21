namespace SlimShader.ResourceDefinition
{
	internal static class EnumExtensions
	{
		public static bool IsMultiSampled(this ResourceDefinitionResourceDimension value)
		{
			switch (value)
			{
				case ResourceDefinitionResourceDimension.Texture2DMultiSampled :
				case ResourceDefinitionResourceDimension.Texture2DMultiSampledArray :
					return true;
				default :
					return false;
			}
		}
	}
}