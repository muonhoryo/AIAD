Shader "Custom/CockpitShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _NormalMap("NormalMap (RGB)",2D)="white" {}
        _AnimationAlphaMap("AnimMap (RGB)",2D)="white"{}
        _Threshold("Threshold",Range(0,1))=1
    }
    SubShader
    {
        Tags {"Queue"="Geometry" "RenderType"="Opaque" }
        Cull Back
        LOD 200
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _NormalMap;
        sampler2D _AnimationAlphaMap;
        half _Threshold;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_NormalMap;
            float2 uv_AnimationAlphaMap;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            half thr = _Threshold; //*0.4;
            bool isPassedWindow = length(tex2D(_AnimationAlphaMap, IN.uv_AnimationAlphaMap).rgb) > thr;
            if (isPassedWindow) {
                o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
                o.Albedo = c.rgb;
                o.Metallic = 1;
                o.Smoothness = 0.5;
                o.Alpha = 1;
            }
            else {
                clip(-1);
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}
