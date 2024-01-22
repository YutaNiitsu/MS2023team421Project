Shader"Unlit/ShieldEffectShader2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed ("Speed", Float) = 0.5
        _Frequency ("Frequency", Float) = 5.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Speed;
            float _Frequency;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
    
   
    float time = fmod(_Time.w * _Speed, _Frequency) - _Frequency * 0.5f;
    float2 vec = float2(-1.0f, 1.0f);
    {
        float a = i.uv.x * vec.x + i.uv.y * vec.y + time;
        float dist = a * a / dot(vec, vec) * 0.5f;
        dist = saturate(dist);
        float powresult = pow(1.0f - dist, 100);
        //col.rgb *= powresult + 1.0f;
        col.rgb = lerp(col.rgb, 1.0f, powresult);
        col.a *= 0.8f;
        col.a = lerp(col.a, 1.0f, powresult);
    }
  
    
    //スペキュラー
    //float4 eyePos = float4(0.0f, 0.0f, -5.0f, 1.0f); //視点座標
    //float4 pntlightPos = float4(time, time * vec.y / vec.x, -10.0f, 1.0f); //点光源座標
    //pntlightPos.x += -vec.x * 0.5f;
    //pntlightPos.y += vec.y * 0.5f;
    //float4 pntlightCol = 1.0f; //点光源の色
    //float3 posw = float3(i.uv.x, i.uv.y, 0.0f);
    //float3 norw = float3(0.0f, 0.0f, -1.0f);
    //float2 dist = pntlightPos.xy - posw.xy;
    //dist = dot(dist, dist);
    //dist = saturate(dist);
    //float spe = pow(1.0f - dist, 10);
    
    //col += spe;
    //col.a += spe;
                return col;
            }
            ENDCG
        }
    }
}
