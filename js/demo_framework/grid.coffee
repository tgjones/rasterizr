---
---

@module "DemoFramework", ->
	class @Grid
		constructor: (element, @width, @height, @cellSize) ->
			paper = Raphael(element, width + 1, height + 1);

			drawLineFunc = (path) ->
				paper.path(path).attr({
					"stroke-width": 1,
					"stroke": "#CCC"
				});

			# Draw gridlines.
			for x in [0..width] by cellSize
				drawLineFunc("M" + (x + 0.5) + " 0L" + (x + 0.5) + " " + height)
			for y in [0..height] by cellSize
				drawLineFunc("M0 " + (y + 0.5) + "L" + width + " " + (y + 0.5))
	
			# Create cells.
			numCellsX = width / cellSize
			numCellsY = height / cellSize
			@cells = new Array();
			for x in [0..numCellsX]
				@cells[x] = new Array()
				for y in [0..numCellsY]
					@cells[x][y] =
						shape: paper.rect((x * cellSize) + 1, (y * cellSize) + 1, cellSize - 1, cellSize - 1).attr({
							"stroke-width": 0,
							"fill": "white"
							})
						color: Nexus.Color.white
		
		getCellColor: (x, y) ->
			@cells[x][y].color
	
		setCellColor: (x, y, color) ->
			@cells[x][y].color = color
			@cells[x][y].shape.attr({
				"fill": "rgba(#{color.r * 255}, #{color.g * 255}, #{color.b * 255}, #{color.a})"
			})
	
		setCellColors: (cells) ->
			for i in [0...cells.length]
				cell = cells[i]
				@setCellColor(cell.x, cell.y, cell.color)