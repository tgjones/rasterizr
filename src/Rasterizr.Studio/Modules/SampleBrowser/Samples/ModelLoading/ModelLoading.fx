struct VS_IN
{
	float3 pos : POSITION;
	float3 normal : NORMAL;
	float2 uv : TEXCOORD;
};

struct PS_IN
{
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	float3 normal : TEXCOORD1;
	float3 worldPos : TEXCOORD2;
};

cbuffer VertexConstantData
{
	float4x4 WorldViewProjection;
	float4x4 World;
}

cbuffer PixelConstantData
{
	float4 LightPos;
}

Texture2D DiffuseTexture;
SamplerState DiffuseSampler;

PS_IN VS(VS_IN input)
{
	PS_IN output = (PS_IN) 0;
	
	output.pos = mul(float4(input.pos,1), WorldViewProjection);
	output.normal = mul(input.normal, (float3x3) World);
	output.worldPos =  mul(float4(input.pos, 1), World).xyz;
	output.uv = input.uv;
	
	return output;
}

float4 PS(PS_IN input) : SV_Target
{
    float3 L = normalize(LightPos.xyz - input.worldPos);
    float3 N = normalize(input.normal);

    float3 diffuseTex = DiffuseTexture.Sample(DiffuseSampler, input.uv).xyz;

    float3 diffuse = diffuseTex * saturate(dot(N, L));

    return float4(diffuse, 1);
}
