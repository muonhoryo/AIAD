

Shader "Custom/ShipSimpleShader"
{
    Properties
    {
        _MainTex("Albedo(RGB)", 2D) = "white" {}
        _NormalMap("NormalMap(RGB)",2D)="white" {}
        _Metallic("Metallic",Range(0,1)) = 0
        _Smoothness("Smoothness",Range(0,1))=0
        _HighLightIntensity("HighLight intensity",Range(0,10)) = 1
        _HighLightColor("HighLight color",Color) = (1,1,1,1)
        _Bias("Bias",Float) = -0.1
        _Scale("Scale",Float) = 0
        _Power("Power",Float) = 1
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque"}
            Cull Back
            LOD 200
            
            CGPROGRAM

            #pragma surface surf Standard fullforwardshadows
            #pragma target 3.0


            sampler2D _MainTex;
            sampler2D _NormalMap;
            struct Input
            {
                float2 uv_MainTex;
                float2 uv_NormalMap;
            };
            half _Metallic;
            half _Smoothness;

            void surf(Input Inp, inout SurfaceOutputStandard o)
            {
                o.Normal = UnpackNormal(tex2D(_NormalMap, Inp.uv_NormalMap));
                fixed4 tex = tex2D(_MainTex, Inp.uv_MainTex);
                o.Albedo = tex.rgb;
                o.Metallic = _Metallic;
                o.Smoothness = _Smoothness;
                o.Alpha = tex.a;
            }
            ENDCG
                Pass{
                    Name "SelectionPass"

                    ZWrite Off
                    Blend SrcAlpha OneMinusSrcAlpha

                    CGPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag

                    #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 normal: NORMAL;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    float intensity : COLOR0;
                };

                float _HighLightIntensity;
                fixed4  _HighLightColor;
                float _Bias;
                float _Scale;
                float _Power;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    if (_Scale > 0) {
                        float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
                        float3 normWorld = normalize(mul(unity_ObjectToWorld, v.normal));
                        float3 I = normalize(worldVertex - _WorldSpaceCameraPos.xyz);
                        o.intensity = saturate(_Bias + _Scale * pow(1.0 + dot(I, normWorld), _Power));
                    }
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col;
                    if (_Scale > 0) {
                    col = _HighLightColor * _HighLightIntensity;
                    col.a = i.intensity;
                    }
                    else {
                        col = fixed4(0, 0, 0, 0);
                    }
                    return col;
                }
                ENDCG
            }
        }
        FallBack "Diffuse"
}
