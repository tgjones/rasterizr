namespace SlimShader.ObjectModel
{
	public enum OperandType
	{
		OPERAND_TYPE_TEMP = 0, // Temporary Register File
		OPERAND_TYPE_INPUT = 1, // General Input Register File
		OPERAND_TYPE_OUTPUT = 2, // General Output Register File
		OPERAND_TYPE_INDEXABLE_TEMP = 3, // Temporary Register File (indexable)
		OPERAND_TYPE_IMMEDIATE32 = 4, // 32bit/component immediate value(s)
		// If for example, operand token bits
		// [01:00]==OPERAND_4_COMPONENT,
		// this means that the operand type:
		// OPERAND_TYPE_IMMEDIATE32
		// results in 4 additional 32bit
		// DWORDS present for the operand.
		OPERAND_TYPE_IMMEDIATE64 = 5, // 64bit/comp.imm.val(s)HI:LO
		OPERAND_TYPE_SAMPLER = 6, // Reference to sampler state
		OPERAND_TYPE_RESOURCE = 7, // Reference to memory resource (e.g. texture)
		OPERAND_TYPE_CONSTANT_BUFFER = 8, // Reference to constant buffer
		OPERAND_TYPE_IMMEDIATE_CONSTANT_BUFFER = 9, // Reference to immediate constant buffer
		OPERAND_TYPE_LABEL = 10, // Label
		OPERAND_TYPE_INPUT_PRIMITIVEID = 11, // Input primitive ID
		OPERAND_TYPE_OUTPUT_DEPTH = 12, // Output Depth
		OPERAND_TYPE_NULL = 13, // Null register, used to discard results of operations
		// Below Are operands new in DX 10.1
		OPERAND_TYPE_RASTERIZER = 14,
		// DX10.1 Rasterizer register, used to denote the depth/stencil and render target resources
		OPERAND_TYPE_OUTPUT_COVERAGE_MASK = 15, // DX10.1 PS output MSAA coverage mask (scalar)
		// Below Are operands new in DX 11
		OPERAND_TYPE_STREAM = 16, // Reference to GS stream output resource
		OPERAND_TYPE_FUNCTION_BODY = 17, // Reference to a function definition
		OPERAND_TYPE_FUNCTION_TABLE = 18, // Reference to a set of functions used by a class
		OPERAND_TYPE_INTERFACE = 19, // Reference to an interface
		OPERAND_TYPE_FUNCTION_INPUT = 20, // Reference to an input parameter to a function
		OPERAND_TYPE_FUNCTION_OUTPUT = 21, // Reference to an output parameter to a function
		OPERAND_TYPE_OUTPUT_CONTROL_POINT_ID = 22,
		// HS Control Point phase input saying which output control point ID this is
		OPERAND_TYPE_INPUT_FORK_INSTANCE_ID = 23, // HS Fork Phase input instance ID
		OPERAND_TYPE_INPUT_JOIN_INSTANCE_ID = 24, // HS Join Phase input instance ID
		OPERAND_TYPE_INPUT_CONTROL_POINT = 25, // HS Fork+Join, DS phase input control points (array of them)
		OPERAND_TYPE_OUTPUT_CONTROL_POINT = 26, // HS Fork+Join phase output control points (array of them)
		OPERAND_TYPE_INPUT_PATCH_CONSTANT = 27, // DS+HSJoin Input Patch Constants (array of them)
		OPERAND_TYPE_INPUT_DOMAIN_POINT = 28, // DS Input Domain point
		OPERAND_TYPE_THIS_POINTER = 29, // Reference to an interface this pointer
		OPERAND_TYPE_UNORDERED_ACCESS_VIEW = 30, // Reference to UAV u#
		OPERAND_TYPE_THREAD_GROUP_SHARED_MEMORY = 31, // Reference to Thread Group Shared Memory g#
		OPERAND_TYPE_INPUT_THREAD_ID = 32, // Compute Shader Thread ID
		OPERAND_TYPE_INPUT_THREAD_GROUP_ID = 33, // Compute Shader Thread Group ID
		OPERAND_TYPE_INPUT_THREAD_ID_IN_GROUP = 34, // Compute Shader Thread ID In Thread Group
		OPERAND_TYPE_INPUT_COVERAGE_MASK = 35, // Pixel shader coverage mask input
		OPERAND_TYPE_INPUT_THREAD_ID_IN_GROUP_FLATTENED = 36, // Compute Shader Thread ID In Group Flattened to a 1D value.
		OPERAND_TYPE_INPUT_GS_INSTANCE_ID = 37, // Input GS instance ID
		OPERAND_TYPE_OUTPUT_DEPTH_GREATER_EQUAL = 38, // Output Depth, forced to be greater than or equal than current depth
		OPERAND_TYPE_OUTPUT_DEPTH_LESS_EQUAL = 39, // Output Depth, forced to be less than or equal to current depth
		OPERAND_TYPE_CYCLE_COUNTER = 40, // Cycle counter
	}
}