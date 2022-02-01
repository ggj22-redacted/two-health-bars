Shader "Retro3D/Skybox"
{
    Properties
    {
        _Cube ("Environment Map", Cube) = "white" {}
        _Color("Tint", Color) = (0.5, 0.5, 0.5)
        [Gamma] _Exposure ("Exposure", Range(0, 8)) = 1.0
    }

    HLSLINCLUDE

    #include "UnityCG.cginc"

    float4 _MainTex_ST;
    half3 _Color;
    half _Exposure;
    samplerCUBE _Cube;

    struct Attributes
    {
        float4 position : POSITION;
        float3 texcoord : TEXCOORD;
    };

    struct Varyings
    {
        float4 position : SV_POSITION;
        noperspective float3 texcoord : TEXCOORD;
    };

    Varyings Vertex(const Attributes input)
    {
        float3 vp = UnityObjectToViewPos(input.position);

        Varyings output;
        output.position = UnityObjectToClipPos(vp);
        output.texcoord = input.texcoord;

        return output;
    }

    fixed4 Fragment(const Varyings input) : SV_Target
    {
        float3 uv = input.texcoord;
        uv = floor(uv * 256) / 256;

        half4 c = texCUBE (_Cube, uv);
        c = floor(c * 16) / 16;
        c.rgb *= _Color * _Exposure;

        return c;
    }

    ENDHLSL

    SubShader
    {
        Pass
        {
            Tags { "Queue" = "Background" }
            ZWrite off
            Cull off
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            ENDHLSL
        }
    }
}
