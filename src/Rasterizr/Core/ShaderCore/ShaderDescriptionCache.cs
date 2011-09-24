using System;
using System.Collections.Generic;

namespace Rasterizr.Core.ShaderCore
{
	public static class ShaderDescriptionCache
	{
		private static readonly Dictionary<Type, ShaderDescription> Cache;

		static ShaderDescriptionCache()
		{
			Cache = new Dictionary<Type, ShaderDescription>();
		}

		public static ShaderDescription GetDescription(IShader shader)
		{
			var shaderType = shader.GetType();
			if (!Cache.ContainsKey(shaderType))
				Cache[shaderType] = new ShaderDescription(shader);
			return Cache[shaderType];
		}
	}
}