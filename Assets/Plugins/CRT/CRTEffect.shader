// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "PostEffects/CRT"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _VignetSize("Vignet Size", Range(0, 1)) = 0.2
        _VignetDarkening("Vignet Darkening", Range(0, 10)) = 1.3
        _ScanlineIntensity("Scanline Intensity", Range(0, 1)) = 1
        _ColorShift("Color Shift", Range(0, 1)) = 0.2
        _NoiseX("NoiseX", Range(0, 1)) = 0
        _Offset("Offset", Vector) = (0, 0, 0, 0)
        _RGBNoise("RGBNoise", Range(0, 1)) = 0
        _SinNoiseWidth("SineNoiseWidth", Float) = 1
        _SinNoiseScale("SinNoiseScale", Float) = 1
        _SinNoiseOffset("SinNoiseOffset", Float) = 1
        _ScanLineTail("Tail", Float) = 0.5
        _ScanLineSpeed("TailSpeed", Float) = 100
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

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

            v2f vert(const appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float rand(float2 co)
            {
                return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
            }

            float2 mod(float2 a, float2 b)
            {
                return a - floor(a / b) * b;
            }

            sampler2D _MainTex;
            float _VignetSize;
            float _VignetDarkening;
            float _ScanlineIntensity;
            float _ColorShift;
            float _NoiseX;
            float2 _Offset;
            float _RGBNoise;
            float _SinNoiseWidth;
            float _SinNoiseScale;
            float _SinNoiseOffset;
            float _ScanLineTail;
            float _ScanLineSpeed;
            float _CurrentTime;

            fixed4 frag(v2f i) : SV_Target
            {
                float2 inUV = i.uv;
                float2 uv = i.uv - 0.5;

                // Recalculate UV coordinates and distort the screen
                const float vignet = length(uv);
                uv /= 1 - vignet * _VignetSize;
                float2 texUV = uv + 0.5;

                // Do not draw outside the screen
                if (max(abs(uv.y) - 0.5, abs(uv.x) - 0.5) > 0)
                {
                    return float4(0, 0, 0, 1);
                }

                // Calculate color
                float3 col;

                // Apply noise and offset
                texUV.x += sin(texUV.y * _SinNoiseWidth + _SinNoiseOffset) * _SinNoiseScale;
                texUV += _Offset;
                texUV.x += (rand(floor(texUV.y * 500) + _CurrentTime) - 0.5) * _NoiseX;
                texUV = mod(texUV, 1);

                // Get color, shift RGB little by little
                const float color_shift = _ColorShift / 100.;
                col.r = tex2D(_MainTex, texUV).r;
                col.g = tex2D(_MainTex, texUV - float2(color_shift, 0)).g;
                col.b = tex2D(_MainTex, texUV - float2(color_shift * 2, 0)).b;

                // RGB noise
                if (rand((rand(floor(texUV.y * 500) + _CurrentTime) - 0.5) + _CurrentTime) < _RGBNoise)
                {
                    col.r = rand(uv + float2(123 + _CurrentTime, 0));
                    col.g = rand(uv + float2(123 + _CurrentTime, 1));
                    col.b = rand(uv + float2(123 + _CurrentTime, 2));
                }

                // Determine the RGB to draw for each pixel
                const float floorX = fmod(inUV.x * _ScreenParams.x / 3, 1);
                col.r *= floorX > 0.3333;
                col.g *= floorX < 0.3333 || floorX > 0.6666;
                col.b *= floorX < 0.6666;

                // Draw scanline
                const float scanLineColor = _ScanlineIntensity * sin(_CurrentTime * 10 + uv.y * 500) / 2 + 0.5;
                col *= 0.5 + clamp(scanLineColor + 0.5, 0, 1) * 0.5;

                // Draw afterimage of scan line
                const float tail = clamp(
                    (frac(uv.y + _CurrentTime * _ScanLineSpeed) - 1 + _ScanLineTail) / min(_ScanLineTail, 1), 0, 1);
                col *= tail;

                // Darken the screen edge
                col *= 1 - vignet * _VignetDarkening;

                return float4(col, 1);
            }
            ENDCG
        }
    }
}