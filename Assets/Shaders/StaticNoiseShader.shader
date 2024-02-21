Shader "Custom/TVStaticShader"
{
    Properties
    {
        _Intensity ("Intensity", Range(0, 1)) = 0.5
        _EvolveSpeed ("Evolve Speed", Range(0.1, 10)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _Intensity;
            float _EvolveSpeed;

            // TV static noise function
            float noise(float2 pos, float evolve) {
                float e = frac((evolve * 0.01));
                float cx = pos.x * e;
                float cy = pos.y * e;
                return frac(23.0 * frac(2.0 / frac(frac(cx * 2.4 / cy * 23.0 + pow(abs(cy / 22.4), 3.3)) * frac(cx * evolve / pow(abs(cy), 0.050)))));
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float staticNoise = noise(uv, _Time.y * _EvolveSpeed);
                
                // Transform intensity range from 0 to 1 to 0.99 to 1, with 0 becoming 1 and 1 becoming 0.99
                float transformedIntensity = 0.99 * (1 - _Intensity) + 1 * _Intensity;
                
                // Adjust the ratio of black and white pixels based on transformed intensity
                float threshold = transformedIntensity;
                float pixelValue = staticNoise > threshold ? 1.0 : 0.0;

                // Ensure black pixels are transparent
                if (pixelValue == 0.0)
                    discard;

                return fixed4(pixelValue, pixelValue, pixelValue, 1.0);
            }
            ENDCG
        }
    }
}