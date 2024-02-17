Shader "Custom/UI/InterfaceBar"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _Mask("Mask",2D) = "white" {}
        _Threshold("Threshold",Range(0,1)) = 1
        _Color("Tint", Color) = (1, 1, 1, 1)
    }

        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
            }

            Cull Off
            Lighting Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                sampler2D _MainTex;
                sampler2D _Mask;
                half _Threshold;
                fixed4 _Color;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 texcoord : TEXCOORD0;
                    fixed4 color : COLOR;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    float2 texcoord : TEXCOORD0;
                    fixed4 color : COLOR;
                };

                v2f vert(appdata IN)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(IN.vertex);
                    o.texcoord = IN.texcoord;
                    o.color = IN.color;

                    return o;
                }

                fixed4 frag(v2f IN) : SV_Target
                {
                    fixed4 color;

                    half len = (tex2D(_Mask, IN.texcoord)).r;
                    if (len >  _Threshold*_Threshold) {
                        color =fixed4(0,0,0,0);
                    }
                    else {
                        color = (tex2D(_MainTex, IN.texcoord))*IN.color * _Color;
                    }
                    return color;
                }
                ENDCG
            }
        }
}