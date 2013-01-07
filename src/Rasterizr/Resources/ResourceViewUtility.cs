using System;

namespace Rasterizr.Resources
{
	internal static class ResourceViewUtility
	{
		 public static Format GetActualFormat(Format descriptionFormat, Resource resource)
		 {
			 if (descriptionFormat == Format.Unknown && !(resource is TextureBase))
				 throw new ArgumentException();

			 return (descriptionFormat == Format.Unknown)
				? ((TextureBase) resource).Format
				: descriptionFormat;
		 }
	}
}