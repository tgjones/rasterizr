---
---

class Nexus.Vector2
	constructor: (@x, @y) ->

class Nexus.Vector3
	constructor: (@x, @y, @z) ->

class Nexus.Color
	constructor: (@r, @g, @b, @a) ->
	
Nexus.Color.multiply = (l, r) ->
	new Nexus.Color(l.r * r.r,
		l.g * r.g,
		l.b * r.b,
		l.a * r.a)