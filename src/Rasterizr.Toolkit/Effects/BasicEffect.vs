#include "BasicEffect.hlsl.h"

struct VertexShaderInput
{
	float3 pos : POSITION;
	float3 normal : NORMAL;
	//float2 uv : TEXCOORD;
};

cbuffer VertexShaderConstants
{
	float4x4 WorldViewProjection;
	float4x4 World;
}

PixelShaderInput main(VertexShaderInput input)
{
	PixelShaderInput output = (PixelShaderInput) 0;
	
	output.pos = mul(float4(input.pos, 1), WorldViewProjection);
	output.worldPos = mul(float4(input.pos, 1), World).xyz;
	output.normal = mul(input.normal, (float3x3) World);
	//output.uv = input.uv;
	
	return output;
}