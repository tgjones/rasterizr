var RasterizrDemos = { };

RasterizrDemos.Grid = function(element, width, height, cellSize) {
	this.width = width;
	this.height = height;
	this.cellSize = cellSize;
	
	var paper = Raphael(element, width + 1, height + 1);

	var drawLineFunc = function(path) {
		paper.path(path).attr({
			"stroke-width": 1,
			"stroke": "#CCC"
		});
	}

	// Draw gridlines.
	for (var x = 0; x <= width; x += cellSize) {
		drawLineFunc("M" + (x + 0.5) + " 0L" + (x + 0.5) + " " + height);
	}
	for (var y = 0; y <= height; y += cellSize) {
		drawLineFunc("M0 " + (y + 0.5) + "L" + width + " " + (y + 0.5));
	}
	
	// Create cells.
	var numCellsX = width / cellSize;
	var numCellsY = height / cellSize;
	var cells = new Array();
	for (x = 0; x < numCellsX; x++) {
		cells[x] = new Array();
		for (y = 0; y < numCellsY; y++) {
			cells[x][y] = paper.rect((x * cellSize) + 1, (y * cellSize) + 1, cellSize - 1, cellSize - 1).attr({
				"stroke-width": 0,
				"fill": "white"
			});
		}
	}
	
	this.setCellColor = function(x, y, color) {
		cells[x][y].attr({
			"fill": color
		})
	};
	
	this.setCellColors = function(cells) {
		for (var i = 0; i < cells.length; i++) {
			var cell = cells[i];
			this.setCellColor(cell.x, cell.y, cell.color);
		}
	}
	
	return this;
};

$(document).ready(function() {
	var cellSize = 20;
	
	var grid1 = new RasterizrDemos.Grid("demo1", 300, 200, cellSize);
	var renderTargetPixels = new Array();
	for (x = 3; x < 8; x++) {
		for (y = 1; y < 8; y++) {
			renderTargetPixels.push({ x: x, y: y, color: "red" });
		}
	}
	grid1.setCellColors(renderTargetPixels);
	
	var grid2 = new RasterizrDemos.Grid("demo2", 300, 200, cellSize);
	var pixels = new Array();
	for (x = 1; x < 14; x++) {
		for (y = 3; y < 6; y++) {
			pixels.push({ x: x, y: y, color: "green" });
		}
	}
	grid2.setCellColors(pixels);
	
	// Now apply some logic to calculate the result colours.
	var grid3 = new RasterizrDemos.Grid("demo3", 300, 200, cellSize);
	grid3.setCellColors(renderTargetPixels);
	for (var i = 0; i < pixels.length; i++) {
		var pixel = pixels[i];
		grid3.setCellColor(pixel.x, pixel.y, pixel.color);
	}
	
});