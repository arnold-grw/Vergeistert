Shader "Unlit/FlashlightHoleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HolePosition ("Hole Position (Viewport XY)", Vector) = (0.5, 0.5, 0, 0)
        _HoleSize ("Hole Size", Float) = 0.2
        _OverlayAlpha ("Overlay Darkness Alpha", Float) = 0.7
        _HoleFeather ("Hole Feather (Softness)", Float) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest Always
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _HolePosition;
            float _HoleSize;
            float _OverlayAlpha;
            float _HoleFeather;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Distance vom UV zum Loch-Zentrum
                float dist = distance(uv, _HolePosition.xy);

                // Alpha abhängig von der Distanz
                float holeMask = 1.0 - smoothstep(_HoleSize, _HoleSize * _HoleFeather, dist);

                // Farbe = schwarz mit Lochmaske
                fixed4 col = fixed4(0, 0, 0, _OverlayAlpha * holeMask);

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
