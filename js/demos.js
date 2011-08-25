function refreshGrids() {
	var renderTargetPixels = new Array();
	for (x = 3; x < 8; x++) {
		for (y = 1; y < 8; y++) {
			renderTargetPixels.push({ x: x, y: y, color: Nexus.Color.red });
		}
	}
	grid1.setCellColors(renderTargetPixels);
	
	var pixels = new Array();
	for (x = 1; x < 14; x++) {
		for (y = 3; y < 6; y++) {
			pixels.push({ x: x, y: y, color: Nexus.Color.green });
		}
	}
	grid2.setCellColors(pixels);
	
	// Now apply some logic to calculate the result colours.
	var colorBlendFunction = $("#blend_operation_rgb").val();
	var blendState = new Rasterizr.OutputMerger.BlendState(
		true, colorBlendFunction, "zero", "one",
		"add", "zero", "one");
	
	grid3.setCellColors(renderTargetPixels);
	for (var i = 0; i < pixels.length; i++) {
		var pixel = pixels[i];
		var destColor = grid1.getCellColor(pixel.x, pixel.y);
		var result = blendState.doBlend(pixel.color, destColor);
		grid3.setCellColor(pixel.x, pixel.y, result);
	}
}

$(document).ready(function() {
	var cellSize = 20;
	
	window.grid1 = new DemoFramework.Grid("demo1", 300, 200, cellSize);
	window.grid2 = new DemoFramework.Grid("demo2", 300, 200, cellSize);
	window.grid3 = new DemoFramework.Grid("demo3", 300, 200, cellSize);
	
	$("#blend_operation_rgb").change(refreshGrids);
	refreshGrids();
});