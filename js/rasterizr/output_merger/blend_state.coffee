---
---

@module "Rasterizr", ->
	@module "OutputMerger", ->
		class @BlendState
			constructor: (@blendEnable, @colorBlendFunction, @colorDestinationBlend, @colorSourceBlend, @alphaBlendFunction, @alphaDestinationBlend, @alphaSourceBlend) ->
			
			doBlend: (source, destination) ->
				result = source;
		
				return result unless @blendEnable
		
				# RGB blending
				colorDestinationBlendFactor = Rasterizr.OutputMerger.Blend[@colorDestinationBlend](source, destination)
				colorSourceBlendFactor = Rasterizr.OutputMerger.Blend[@colorSourceBlend](source, destination)
		
				colorDestination = Nexus.Color.multiply(destination, colorDestinationBlendFactor)
				colorSource = Nexus.Color.multiply(source, colorSourceBlendFactor)
		
				colorBlendFunction = Rasterizr.OutputMerger.BlendFunction[@colorBlendFunction]
				result.r = colorBlendFunction(colorSource.r, colorDestination.r)
				result.g = colorBlendFunction(colorSource.g, colorDestination.g)
				result.b = colorBlendFunction(colorSource.b, colorDestination.b)
		
				# Alpha blending
				alphaDestinationBlendFactor = Rasterizr.OutputMerger.Blend[@alphaDestinationBlend](source, destination)
				alphaSourceBlendFactor = Rasterizr.OutputMerger.Blend[@alphaSourceBlend](source, destination)
		
				alphaDestination = destination.a * alphaDestinationBlendFactor.a
				alphaSource = source.a * alphaSourceBlendFactor.a
		
				alphaBlendFunction = Rasterizr.OutputMerger.BlendFunction[@alphaBlendFunction]
				result.a = alphaBlendFunction(alphaSource, alphaDestination)
		
				result
