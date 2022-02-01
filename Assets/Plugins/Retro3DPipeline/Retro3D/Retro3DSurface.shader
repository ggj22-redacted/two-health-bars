Shader "Retro3D/Surface"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Scale("Scale", Vector) = (1, 1, 0, 0)
        _Color("Tint", Color) = (0.5, 0.5, 0.5)
        [Gamma] _Exposure ("Exposure", Range(0, 8)) = 1.0
        [HDR] _Emission("Color", Color) = (0, 0, 0)
        [Toggle] _useAffineTextureWarping ("Use Affine Texture Warping", Float) = 1
        [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 0
        _CutOff("Cut off", float) = 0.1
    }

    HLSLINCLUDE

    #pragma shader_feature _useAffineTextureWarping

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    float4 _MainTex_ST;
    float2 _Scale;
    half3 _Color;
    half _Exposure;
    fixed4 _Emission;
    uniform float _CutOff;

    struct Attributes
    {
        float4 position : POSITION;
        float2 texcoord : TEXCOORD;
    };

    struct Varyings
    {
        float4 position : SV_POSITION;
        /*noperspective*/ float2 texcoord : TEXCOORD;
        UNITY_FOG_COORDS(1)
    };

    Varyings Vertex(Attributes input)
    {
        float3 vp = UnityObjectToViewPos(input.position.xyz);
        vp = floor(vp * 64) / 64;
        Varyings output;
        output.position = UnityViewToClipPos(vp);
        output.texcoord = TRANSFORM_TEX(input.texcoord * _Scale, _MainTex)
        #ifdef _useAffineTextureWarping
        * output.position.w
        #endif
        ;
        UNITY_TRANSFER_FOG(output, output.position);
        return output;
    }

    half4 Fragment(Varyings input) : SV_Target
    {
        float2 uv = input.texcoord;
        uv = floor(uv * 256) / 256;
        half4 c = tex2D(_MainTex, uv
            #ifdef _useAffineTextureWarping
            / input.position.w
            #endif
            );
        c = floor(c * 16) / 16;
        if (c.a < _CutOff)
            discard;
        c.rgb *= _Color * _Exposure;
        c.rgb += _Emission;
        UNITY_APPLY_FOG(input.fogCoord, c);
        return c;
    }

    ENDHLSL

    SubShader
    {
        Pass
        {
            Tags { "LightMode" = "Base" }
            Cull [_Cull]
            HLSLPROGRAM
            #pragma multi_compile_fog
            #pragma vertex Vertex
            #pragma fragment Fragment
            ENDHLSL
        }
    }
}
