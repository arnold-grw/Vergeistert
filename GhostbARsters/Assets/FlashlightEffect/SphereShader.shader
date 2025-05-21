// Upgrade NOTE: commented out 'float3 _WorldSpaceCameraPos', a built-in variable

Shader "Unlit/SphereShader"
{
    Properties
    {
        _LightDir ("Lichtrichtung (normalisiert)", Vector) = (0, 0, -1, 0)
        _HoleSize ("Loch-Größe (0..1)", Float) = 0.0
        _Smoothness ("Kantenglättung (0..1)", Float) = 0.45
        _OverlayAlpha ("Dunkelheit", Float) = 0.97
    }
    SubShader
    {
        Tags { "Queue"="Geometry+2" "RenderType"="Transparent" }
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Front

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float3 _LightDir;
            float _HoleSize;
            float _Smoothness;
            float _OverlayAlpha;

            // float3 _WorldSpaceCameraPos;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 viewDir : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = normalize(worldPos - _WorldSpaceCameraPos);
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 lightDir = normalize(_LightDir);
                float dist = distance(normalize(i.viewDir), lightDir); // 0 = exakt mittig, 1 = ganz außen

                float inner = _HoleSize;
                float outer = _HoleSize + _Smoothness;

                float alphaFactor = smoothstep(inner, outer, dist);
                float alpha = _OverlayAlpha * alphaFactor;

                return float4(0, 0, 0, alpha);
            }
            ENDCG
        }
    }
}
