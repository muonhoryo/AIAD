
Shader "Custom/CockpitShader_Transparent"{

    Properties
    {
        _GlassAlphaMap("GlassMap (RGB)",2D) = "white"{}
        _AnimationAlphaMap("AnimMap (RGB)",2D) = "white"{}
        _Threshold("Threshold",Range(0,1)) = 1
    }
        SubShader
        {
            Tags {"Queue" = "Transparent+1" "RenderType" = "Transparent" }
            Cull Back
            LOD 200
            CGPROGRAM
            #pragma surface surf Standard fullforwardshadows alpha:fade

            #pragma target 3.0

            sampler2D _GlassAlphaMap;
            sampler2D _AnimationAlphaMap;
            half _Threshold;

            struct Input
            {
                float2 uv_MainTex;
                float2 uv_GlassAlphaMap;
                float2 uv_AnimationAlphaMap;
            };

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                half thr = _Threshold * 0.4;
                fixed4 col = tex2D(_GlassAlphaMap, IN.uv_GlassAlphaMap);
                bool isPassedWindow = length(tex2D(_AnimationAlphaMap, IN.uv_AnimationAlphaMap).rgb) <= thr;
                bool isPassedGlassTexture = col.rgb != fixed3(0, 0, 0);
                bool isPassed = isPassedWindow && isPassedGlassTexture;
                if (isPassed) {
                    o.Albedo = fixed3(0,0,0);
                    o.Alpha = length(col.rgb);
                }
                else {
                    clip(-1);
                }
            }
            ENDCG
        }
            FallBack "Diffuse"
}