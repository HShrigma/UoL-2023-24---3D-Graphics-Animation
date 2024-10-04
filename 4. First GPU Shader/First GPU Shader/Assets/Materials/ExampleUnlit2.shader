Shader "Unlit/ExampleUnlit2"
{
    Properties
    {
        _MainTexture("Main Texture", 2D) = "white"{}
        _AnimateXY("Animate X Y", Vector) = (0,0,0,0)
        _MinVert("Min Animate Movement", Vector) = (0,0,0,0)
        _MaxVert("Max Animate Movement", Vector) = (0,0,0,0)
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTexture;
            float4 _MainTexture_ST;
            float4 _AnimateXY;
            float4 _MinVert;
            float4 _MaxVert;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex.xyz += o.vertex * _AnimateXY.xyz * clamp(sin(_Time.y),0,1);
                o.uv = TRANSFORM_TEX(v.uv, _MainTexture);
                o.uv += frac(_AnimateXY.xy *_MainTexture_ST.xy *_Time.yy);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uvs = i.uv;
                fixed4 textureColor = tex2D(_MainTexture, uvs);                
                return textureColor;
            }
            ENDCG
        }
    }
}
