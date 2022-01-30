Shader "Retro3D/Skybox"
{
    Properties
    {
        _Cube ("Environment Map", Cube) = "white" {}
        _Color("Tint", Color) = (0.5, 0.5, 0.5)
    }

    HLSLINCLUDE

    #include "UnityCG.cginc"

    float4 _MainTex_ST;
    half3 _Color;
    uniform float _CutOff;
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

    Varyings Vertex(Attributes input)
    {
        float3 vp = UnityObjectToViewPos(input.position.xyz);
        
        Varyings output;
        output.position = UnityObjectToClipPos(vp);
        output.texcoord = input.texcoord;
        return output;
    }

    fixed4 Fragment(Varyings input) : SV_Target
    {
        float2 uv = input.texcoord;
        uv = floor(uv * 256) / 256;
        half4 c = texCUBE (_Cube, input.texcoord);
        c = floor(c * 16) / 16;
        c.rgb *= _Color;

        return c;
    }

    ENDHLSL

    SubShader
    {
        Pass
        {
            Tags { "Queue"="Background" "LightMode" = "Back" }
            ZWrite off
            Cull off
            HLSLPROGRAM
            #pragma multi_compile_fog
            #pragma vertex Vertex
            #pragma fragment Fragment
            ENDHLSL
        }
    }
}
