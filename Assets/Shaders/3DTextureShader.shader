Shader "Custom/9Slice"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        // Sliders for center and edge sizes
        _CenterX ("Center Size X", Range(0, 1)) = 0.5
        _CenterY ("Center Size Y", Range(0, 1)) = 0.5
        _EdgeX ("Edge Size X", Range(-0.5, 0.5)) = 0.1
        _EdgeY ("Edge Size Y", Range(-0.5, 0.5)) = 0.1

        // Tiling options for texture
        _Tiling ("Tiling", Vector) = (1, 1, 0, 0)
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

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float _CenterX, _CenterY;
            float _EdgeX, _EdgeY;
            float4 _Tiling;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // Calculate UV mapping based on texture slicing
                float2 uv = v.uv;
                float2 centerSize = float2(_CenterX, _CenterY);
                float2 edgeSize = float2(_EdgeX, _EdgeY);
                
                // Center scaling
                if (uv.x > edgeSize.x && uv.x < 1.0 - edgeSize.x &&
                    uv.y > edgeSize.y && uv.y < 1.0 - edgeSize.y)
                {
                    uv = (uv - edgeSize) / (1.0 - 2.0 * edgeSize);
                }
                // Edge scaling
                else if (uv.x <= edgeSize.x || uv.x >= 1.0 - edgeSize.x ||
                         uv.y <= edgeSize.y || uv.y >= 1.0 - edgeSize.y)
                {
                    uv = (uv - edgeSize) / (1.0 - 2.0 * edgeSize);
                }

                // Apply tiling
                uv *= _Tiling.xy;

                o.uv = uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
