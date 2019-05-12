Shader "Hidden/RangeIndicator"
{
    Properties
    {
        partCount ("partCount", int) = 36
        
        visualRate ("visualRate", float) = 0.5
        fadeRate ("fadeRate", float) = 0.1
        
        width ("width", float) = 0.04
        radius ("radius", float) = 0.9
        
        angleRange ("angleRange", float) = 0.78539816339744830961566084581988
        currentDir ("currentDir", vector) = (0, 0, 0, 0)
    }
    
    SubShader
    {
        // No culling or depth
        Cull Off
        ZWrite Off
        ZTest Always

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
                float4 color : COLOR0;
                float2 uv : TEXCOORD0;
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
            
            int partCount;
            float visualRate;
            float fadeRate;
            float width;
            float radius;
            float angleRange;
            float4 currentDir;
            
            fixed4 frag (v2f i) : SV_Target
            {
                float pi = 3.141592653589793;
                float2 pos = (i.uv - float2(0.5, 0.5));
                float dist = length(pos);
                float partAngle = 2.0 * pi / partCount;
                float curAngle = atan2(pos.y, pos.x);
                float curPartAngle = (curAngle % partAngle) / partAngle;
                if(curPartAngle < 0.0) curPartAngle += 1.0;
                
                ///////////////////////////////////////////////////////////////
                if(curPartAngle < 0.5 - 0.5 * (visualRate + fadeRate)) discard;
                if(curPartAngle > 0.5 + 0.5 * (visualRate + fadeRate)) discard;
                
                float alphaTangential = curPartAngle < 0.5 - 0.5 * visualRate || curPartAngle > 0.5 + 0.5 * visualRate ?
                    1.0 - (abs(curPartAngle - 0.5) - 0.5 * visualRate) / (0.5 * fadeRate)
                    : 1.0;
                
                alphaTangential = pow(alphaTangential, 1.5);
                
                ///////////////////////////////////////////////////////////////
                if(dist < radius - 0.5 * width) discard;
                if(dist > radius + 0.5 * width) discard;
                
                float alphaRadial = 1.0 - abs(radius - dist) / (0.5 * width);
                alphaRadial = pow(alphaRadial, 1.5);
                
                ///////////////////////////////////////////////////////////////
                currentDir = currentDir / length(currentDir);
                float deltaAngle = acos(dot(currentDir.xy, pos.xy) / length(currentDir.xy) / length(pos.xy));
                float alphaAngle = 1.0 - min(1.0, abs(deltaAngle) / angleRange);
                alphaAngle = pow(alphaAngle, 1.5);
                
                ///////////////////////////////////////////////////////////////
                float alpha = alphaTangential * alphaRadial * alphaAngle;
                
                return float4(i.color.rgb, i.color.a * alpha);
            }
            ENDCG
        }
    }
}
