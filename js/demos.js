$(document).ready(function() {
	var gridSize = { width: 300, height: 200 };
	
	var paper = Raphael("demo1", gridSize.width + 1, gridSize.height + 1);
	
	var drawLineFunc = function(path) {
		paper.path(path).attr({
			"stroke-width": 1,
			"stroke": "#CCC"
		});
	}
	
	// Draw gridlines.
	var cellSize = 20;
	for (var x = 0; x <= gridSize.width; x += cellSize) {
		drawLineFunc("M" + (x + 0.5) + " 0L" + (x + 0.5) + " " + gridSize.height);
	}
		
	for (var y = 0; y <= gridSize.height; y += cellSize) {
		drawLineFunc("M0 " + (y + 0.5) + "L" + gridSize.width + " " + (y + 0.5));
	}
	
});