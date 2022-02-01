Shader "Retro3D/Surface (Transparent)"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Tint", Color) = (0.5, 0.5, 0.5, 1)
        [Gamma] _Exposure ("Exposure", Range(0, 8)) = 1.0
        [HDR] _Emission("Color", Color) = (0, 0, 0)
        [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 0
    }

    HLSLINCLUDE
    #include "UnityCG.cginc"

    sampler2D _MainTex;
    float4 _MainTex_ST;
    half4 _Color;
    half _Exposure;
    fixed4 _Emission;

    struct Attributes
    {
        float4 position : POSITION;
        float2 texcoord : TEXCOORD;
    };

    struct Varyings
    {
        float4 position : SV_POSITION;
        noperspective float2 texcoord : TEXCOORD;
        UNITY_FOG_COORDS(1)
    };

    Varyings Vertex(Attributes input)
    {
        float3 vp = UnityObjectToViewPos(input.position.xyz);
        vp = floor(vp * 64) / 64;
        Varyings output;
        output.position = UnityViewToClipPos(vp);
        output.texcoord = TRANSFORM_TEX(input.texcoord, _MainTex);
        UNITY_TRANSFER_FOG(output, output.position);
        return output;
    }

    half4 Fragment(Varyings input) : SV_Target
    {
        float2 uv = input.texcoord;
        uv = floor(uv * 256) / 256;
        half4 c = tex2D(_MainTex, uv);
        c = floor(c * 16) / 16;
        c.rgba *= _Color;
        c.rgb *= _Exposure;
        c.rgb += _Emission;
        UNITY_APPLY_FOG(input.fogCoord, c);
        return c;
    }
    ENDHLSL

    SubShader
    {
        Pass
        {
            Tags
            {
                "Queue" = "Transparent" "LightMode" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"
            }
            Cull [_Cull]
            ZWrite off
            Blend SrcAlpha OneMinusSrcAlpha
            HLSLPROGRAM
            #pragma multi_compile_fog
            #pragma vertex Vertex alpha
            #pragma fragment Fragment alpha
            ENDHLSL
        }
    }
}