---
---

@module "Nexus", ->
	class @Vector2
		constructor: (@x, @y) ->

	class @Vector3
		constructor: (@x, @y, @z) ->

	class @Color
		constructor: (@r, @g, @b, @a) ->
		
		@multiply: (l, r) ->
			new Color(l.r * r.r,
				l.g * r.g,
				l.b * r.b,
				l.a * r.a)
		
		@red: new Color(1, 0, 0, 1)
		@green: new Color(0, 1, 0, 1)
		@white: new Color(1, 1, 1, 1)