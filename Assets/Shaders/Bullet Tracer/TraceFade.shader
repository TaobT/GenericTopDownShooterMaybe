Shader "Custom/TraceFade" {
    Properties {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _TintColor ("Tint Color", Color) = (1, 1, 1, 1)
        _Fade ("Fade", Range(0, 1)) = 0.5
    }

    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            sampler2D _MainTex;
            float4 _TintColor;
            float _Fade;

            float4 frag (v2f i) : SV_Target {
                float4 col = tex2D(_MainTex, i.uv) * i.color * _TintColor;
                col.a *= (1 - i.uv.x * _Fade);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}