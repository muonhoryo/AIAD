Shader "Custom/LightingShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        fixed4 _Color;
    struct Input 
    {
        float2 uv_MainTex;
    };

        void surf (Input i, inout SurfaceOutputStandard o)
        {
            o.Albedo = _Color;
            o.Metallic = 0;
            o.Smoothness = 0;
            o.Emission = _Color.rgb;
            o.Alpha = _Color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
