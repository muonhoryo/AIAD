// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/StarsShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)(Name=_MainTex)", 2D) = "white" {}
        _AlphaMap("AlphaMap(Name=_AlphaMap)", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"Queue" = "Transparent+1" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        LOD 100
        Pass
        {
            CGPROGRAM

            #pragma vertex vert alpha
            #pragma fragment frag alpha

            #include "UnityCG.cginc"


            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex  : SV_POSITION;
                half2 texcoord : TEXCOORD0;
                half2 texcoord1 : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _AlphaMap;
            float4 _MainTex_ST;
            float4 _AlphaMap_ST;
            float4 _Color;

            v2f vert(appdata_t v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                //v.texcoord.x = 1 - v.texcoord.x;
                //v.texcoord1.x = 1 - v.texcoord1.x;
                o.texcoord1= TRANSFORM_TEX(v.texcoord1, _AlphaMap);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = (tex2D(_MainTex, i.texcoord) *tex2D(_AlphaMap,i.texcoord1))* _Color; // multiply by _Color
                return col;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}
