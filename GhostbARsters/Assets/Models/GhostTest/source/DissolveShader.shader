Shader "Custom/DissolveShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _DissolveAmount ("Dissolve Amount", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        CGPROGRAM
        #pragma surface surf Standard addshadow
        sampler2D _MainTex;
        sampler2D _NoiseTex;
        float _DissolveAmount;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_NoiseTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float noise = tex2D(_NoiseTex, IN.uv_NoiseTex).r;
            clip(noise - _DissolveAmount); // Pixel abschneiden, wenn Noise kleiner ist

            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = 1; // wichtig für opaque korrektes Z-Sorting
        }
        ENDCG
    }
    FallBack "Diffuse"
}
