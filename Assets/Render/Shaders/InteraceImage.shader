Shader "Custom/UI/InterfaceImage"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1, 1, 1, 1)
        _StencilRef("StencilRef",Integer) = 0
        _StencilComp("StencilComparasion",Integer)=0
        _StencilReadMask("StencilReadMask",Integer)=0
        _StencilWriteMask("StencilWriteMask",Integer) = 0
        _StencilOpeation("StencilOperation",Integer) = 0
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
                    Stencil
                {
                    Ref[_StencilRef]
                        Comp[_StencilComp]
                        ReadMask[_StencilReadMask]
                        WriteMask[_StencilWriteMask]
                        Pass[_StencilOpeation]
                }
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag


                sampler2D _MainTex;
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
                    fixed4 color=(tex2D(_MainTex, IN.texcoord)) * IN.color * _Color;
                    return color;
                }
                    ENDCG

            }
        }
}