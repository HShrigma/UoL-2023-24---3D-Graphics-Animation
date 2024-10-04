Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DiffuseColor ("Diffuse Color", Color) = (1,1,1,1)
        _SpecularColor ("Specular Color", Color) = (1,1,1,1)
        _Shine ("Shine", Range(0.1, 100)) = 1
        _DiffuseCoeff ("Diffuse Coefficient", Range(0,1)) = 1
        _SpecularCoeff ("Specular Coefficient", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal: NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 viewT : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _DiffuseColor;
            float4 _SpecularColor;
            float _Shine;
            float _DiffuseCoeff;
            float _SpecularCoeff;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = v.normal;
                o.viewT = normalize(WorldSpaceViewDir(v.vertex));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // _WorldSpaceLightPos0 provided by Unity
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

                // get dot product between surface normal and light direction
                float lightDot = dot(i.normal, lightDir);
                float diffuse = _DiffuseCoeff * lightDot;
                // get direction to the camera
                float3 get_cam =  i.viewT;
                // get bisection: h = (e + l) / ||e + l||
                float3 bisect = (-1*lightDir + get_cam)/(normalize(lightDir*-1 + get_cam));
                float specDot = pow(dot(bisect, i.normal), _Shine);
                float specular = _SpecularCoeff * specDot;
                // sample texture for color
                float3 color = tex2D(_MainTex, i.uv.xy).rgb;

                // add ambient light
                //color += ShadeSH9(half4(i.normal, 1));
                float mod = _LightColor0 * (diffuse + specular);
                // multiply albedo and lighting
                float3 rgb = color * mod;
                //my code
                //end my code
                float4 finalColor = float4(rgb, 1.0);

                finalColor *= _DiffuseColor;
                finalColor *= _SpecularColor;
                return finalColor;
            }
            ENDCG
        }
    }
}
