---
---

@module "Rasterizr", ->
	@module "OutputMerger", ->
		@Blend =
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