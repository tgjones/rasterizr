namespace Rasterizr.Resources
{
	internal static class ResourceViewUtility
	{
		 public static Format GetActualFormat(Format descriptionFormat, Resource resource)
		 {
			 return (descriptionFormat == Format.Unknown)
				? ((TextureBase) resource).Format
				: descriptionFormat;
		 }
	}
}