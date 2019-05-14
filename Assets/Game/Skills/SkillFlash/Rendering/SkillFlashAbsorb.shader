Shader "Hidden/SkillFlashAbsorb"
{
    Properties
    {
        width ("width", float) = 0.05
        radius ("radius", float) = 0.8
        baseColor ("baseColor", vector) = (1, 1, 1, 1)
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        
        Blend SrcAlpha OneMinusSrcAlpha
        
        
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
                float4 color : COLOR0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 color : COLOR0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            float width;
            float radius;
            float4 baseColor;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 pos = (i.uv - float2(0.5, 0.5));
                float dist = sqrt(pos.x * pos.x + pos.y * pos.y);
                // if(dist < radius- width || dist > radius + width) discard;
                float rate = abs(radius - dist) / width;
                rate = pow(rate, 1.5);
                float illu = 1.0 - rate;
                float4 color = float4(baseColor.rgb, illu) * i.color;
                return color;
            }
            ENDCG
        }
    }
}
