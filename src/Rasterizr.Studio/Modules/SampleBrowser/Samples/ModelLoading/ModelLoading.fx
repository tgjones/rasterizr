struct VS_IN
{
    float3 position : POSITION;
	float3 normal : NORMAL;
	float2 uv : TEXCOORD;
};

struct PS_IN
{
    float4 position : SV_POSITION;
    float3 normal : TEXCOORD0;
    float2 uv : TEXCOORD1;
};

cbuffer VertexConstantData
{
	float4x4 WorldViewProjection;
	float4x4 World;
}

cbuffer PixelConstantData
{
	float4 LightDirection;
}

Texture2D DiffuseTexture;
SamplerState DiffuseSampler;

PS_IN VS(VS_IN input)
{
	PS_IN output = (PS_IN) 0;
	
    output.position = mul(float4(input.position, 1), WorldViewProjection);
	output.normal = mul(input.normal, (float3x3) World);
	output.uv = input.uv;
	
	return output;
}

float4 PS(PS_IN input) : SV_Target
{
    float3 L = LightDirection.xyz;
    float3 N = normalize(input.normal);

    float3 diffuseTex = DiffuseTexture.Sample(DiffuseSampler, input.uv).xyz;

    float3 diffuse = diffuseTex * saturate(dot(N, L));

    return float4(diffuse, 1);
}
