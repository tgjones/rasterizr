using System;
using System.Collections.Generic;

namespace Rasterizr.InputAssembler
{
	public static class InputLayoutDescriptionCache
	{
		private static readonly Dictionary<CacheKey, InputLayoutDescription> Cache;

		static InputLayoutDescriptionCache()
		{
			Cache = new Dictionary<CacheKey, InputLayoutDescription>();
		}

		public static InputLayoutDescription GetDescription(InputLayout inputLayout, Type vertexType)
		{
			var cacheKey = new CacheKey { InputLayout = inputLayout, VertexType = vertexType};
			if (!Cache.ContainsKey(cacheKey))
				Cache[cacheKey] = new InputLayoutDescription(inputLayout, vertexType);
			return Cache[cacheKey];
		}

		private struct CacheKey
		{
			public InputLayout InputLayout;
			public Type VertexType;
		}
	}
}