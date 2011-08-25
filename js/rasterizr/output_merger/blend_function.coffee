---
---

@module "Rasterizr", ->
	@module "OutputMerger", ->
		@BlendFunction =
			add: (s, d) -> s + d
			max: (s, d) -> Math.max(s, d)
			min: (s, d) -> Math.min(s, d)
			reverseSubtract: (s, d) -> d - s
			subtract: (s, d) -> s - d