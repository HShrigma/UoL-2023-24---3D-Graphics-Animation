#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"

struct MeshData
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float2 uv : TEXCOORD0;
    float4 tangentOS : TANGENT;
};

struct Interpolators
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
    float3 normal : TEXCOORD1;
    float3 tangentWS : TEXCOORD2;
    half3 bitangentWS : TEXCOORD3;
    float3 wPos : TEXCOORD4;
};

sampler2D _MainTex;
sampler2D _HeightMap;
float _HeightStr;
sampler2D _NormalTex;
float _NormalStr;
float4 _MainTex_ST;
//implement Base Color from inspector
float4 _BaseColor;
//implement Specular Color from inspector
float4 _SpecularColor;
float _Gloss;

Interpolators vert(MeshData v)
{
    Interpolators o;
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    //sample height map
    float height = tex2D(_HeightMap, o.uv).x;
    
    v.vertex.xyz += (v.normal * height);
    
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.normal = UnityObjectToWorldNormal(v.normal);
    o.wPos = mul(unity_ObjectToWorld, v.vertex);
    o.tangentWS = UnityObjectToWorldDir(v.tangentOS.xyz);
    o.bitangentWS = cross(o.normal, o.tangentWS) * (v.tangentOS.w * unity_WorldTransformParams.w);
    return o;
}

fixed4 frag(Interpolators i) : SV_Target
{
    //get normal map
    float3 tsNormal = UnpackNormal(tex2D(_NormalTex, i.uv));
    float3x3 mtxTangToWorld = { 
        i.tangentWS.x, i.bitangentWS.x, i.normal.x,
        i.tangentWS.y, i.bitangentWS.y, i.normal.y,
        i.tangentWS.z, i.bitangentWS.z, i.normal.z
        };
    //get normal
    float3 norm = mul(mtxTangToWorld,tsNormal * _NormalStr);
    //Diffuse Lighting

    //get light dir
    float3 L = _WorldSpaceLightPos0.xyz;
    //get dot product
    float3 lambert = saturate(dot(norm, L));
    float3 DiffuseLight = lambert * _LightColor0.xyz;

    //Specular Lighting
    //get dir to eye 
    float3 V = normalize(_WorldSpaceCameraPos - i.wPos);
    //get H
    float3 bisect = normalize(L + V);
    //get specular + Blinn Phong
    float3 specularLight = saturate(dot(bisect, norm)) * (lambert > 0);
    //get m 
    float3 specularExp = exp2(_Gloss * 11) + 2;
    //get dot power of m
    specularLight = pow(specularLight, specularExp);
    //get specular light times intensity
    specularLight *= _LightColor0.xyz;
    specularLight *= _SpecularColor.xyz;
    // sample the texture
    fixed4 col = tex2D(_MainTex, i.uv);
    //implement color switch and lighting
    col *= float4(DiffuseLight * _BaseColor + specularLight, 1.0);
    return col;
}
