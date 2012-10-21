namespace SlimShader.ResourceDefinition
{
	internal static class EnumExtensions
	{
		public static bool IsMultiSampled(this ResourceDimension value)
		{
			switch (value)
			{
				case ResourceDimension.Texture2DMultiSampled :
				case ResourceDimension.Texture2DMultiSampledArray :
					return true;
				default :
					return false;
			}
		}
	}
}