---
---

Rasterizr.BlendFunction =
	add: (s, d) -> s + d
	max: (s, d) -> Math.max(s, d)
	min: (s, d) -> Math.min(s, d)
	reverseSubtract: (s, d) -> d - s
	subtract: (s, d) -> s - d

Rasterizr.Blend =
	zero: (s, d) -> new Nexus.Color(0, 0, 0, 0)
	one: (s, d) -> new Nexus.Color(1, 1, 1, 1)
	sourceColor: (s, d) -> new Nexus.Color(s.r, s.g, s.b, s.a)
	inverseSourceColor: (s, d) -> new Nexus.Color(1 - s.r, 1 - s.g, 1 - s.b, 1 - s.a)
	sourceAlpha: (s, d) -> new Nexus.Color(s.a, s.a, s.a, s.a)
	inverseSourceAlpha: (s, d) -> new Nexus.Color(1 - s.a, 1 - s.a, 1 - s.a, 1 - s.a)
	destinationAlpha:  (s, d) -> new Nexus.Color(d.a, d.a, d.a, d.a)
	inverseDestinationAlpha:  (s, d) -> new Nexus.Color(1 - d.a, 1 - d.a, 1 - d.a, 1 - d.a)
	destinationColor: (s, d) -> new Nexus.Color(d.r, d.g, d.b, d.a)
	inverseDestinationColor: (s, d) -> new Nexus.Color(1 - d.r, 1 - d.g, 1 - d.b, 1 - d.a)

class Rasterizr.BlendState
	constructor: (@blendEnable, @colorBlendFunction, @colorDestinationBlend, @colorSourceBlend, @alphaBlendFunction, @alphaDestinationBlend, @alphaSourceBlend) ->
			
	doBlend: (source, destination) ->
		result = source;
		
		return result unless @blendEnable
		
		# RGB blending
		colorDestinationBlendFactor = Rasterizr.Blend[@colorDestinationBlend](source, destination)
		colorSourceBlendFactor = Rasterizr.Blend[@colorSourceBlend](source, destination)
		
		colorDestination = Nexus.Color.multiply(destination, colorDestinationBlendFactor)
		colorSource = Nexus.Color.multiply(source, colorSourceBlendFactor)
		
		colorBlendFunction = Rasterizr.BlendFunction[@colorBlendFunction]
		result.r = colorBlendFunction(colorSource.r, colorDestination.r)
		result.g = colorBlendFunction(colorSource.g, colorDestination.g)
		result.b = colorBlendFunction(colorSource.b, colorDestination.b)
		
		# Alpha blending
		alphaDestinationBlendFactor = Rasterizr.Blend[@alphaDestinationBlend](source, destination)
		alphaSourceBlendFactor = Rasterizr.Blend[@alphaSourceBlend](source, destination)
		
		alphaDestination = destination.a * alphaDestinationBlendFactor.a
		alphaSource = source.a * alphaSourceBlendFactor.a
		
		alphaBlendFunction = Rasterizr.BlendFunction[@alphaBlendFunction]
		result.a = alphaBlendFunction(alphaSource, alphaDestination)
		
		result
		