Shader "Green" {
    SubShader{
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry+1"}
        Pass {
            Stencil {
                Ref 2
                Comp NotEqual
                Pass keep
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            struct appdata {
                float4 vertex : POSITION;
            };
            struct v2f {
                float4 pos : SV_POSITION;
            };
            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            half4 frag(v2f i) : SV_Target {
                return half4(0,1,0,1);
            }
            ENDCG
        }
    }
}