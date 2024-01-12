Shader "Unlit/ConstellationLineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

// 2D→2D疑似乱数
float2 random22(in float2 vec)
{
    vec = float2(dot(vec, float2(127.1, 311.7)), dot(vec, float2(269.5, 183.3)));

    return frac(sin(vec) * (4378.545));
}
// パーリンノイズ2D
float perlinNoise2(in float2 vec)
{
    float2 ivec = floor(vec);
    float2 fvec = frac(vec);

    float a = dot(random22(ivec + float2(0.0, 0.0)) * 2.0 - 1.0, fvec - float2(0.0, 0.0));
    float b = dot(random22(ivec + float2(1.0, 0.0)) * 2.0 - 1.0, fvec - float2(1.0, 0.0));
    float c = dot(random22(ivec + float2(0.0, 1.0)) * 2.0 - 1.0, fvec - float2(0.0, 1.0));
    float d = dot(random22(ivec + float2(1.0, 1.0)) * 2.0 - 1.0, fvec - float2(1.0, 1.0));

    fvec = smoothstep(0.0, 1.0, fvec);

    return lerp(lerp(a, b, fvec.x), lerp(c, d, fvec.x), fvec.y);
}
// フラクタルパーリンノイズ2D
float fbm2(in float2 vec, in int octave, in float2 offset = 0.0)
{
    float value = 0.0;
    float amplitude = 1.0;

    for (int i = 0; i < octave; i++)
    {
        value += amplitude * perlinNoise2(vec + offset);
        vec *= 2.0;
        amplitude *= 0.5;
    }

    return value;
}

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
    float2 pos = i.vertex.xy;
    pos.xy += _Time * 10.0f;
    
    //col.r = fbm2(pos * 0.1f, 5);
   
    //パーリンノイズ
    float noise = saturate(saturate(fbm2(pos * 0.02f, 5)) + 0.3f);
    float noise2 = saturate(saturate(fbm2(pos * 0.03f, 2)) + 0.3f) + _SinTime;
    //col.a = noise;
     //線の中心に近いほど濃くなる
    col.a = pow(saturate(1.0f - abs(i.uv.y - 0.5f * noise) * 2.0f), 5);
    col.r = 1.1f - pow(saturate(1.0f - abs(i.uv.y - 0.5f * noise) * 2.0f), 5);
    //col.rg = 1.1f - pow(saturate(1.0f - abs(i.uv.y - 0.5f * noise2) * 2.0f), 5);
    col.a += pow(saturate(0.9f - abs(i.uv.y - 0.5f) * 2.0f), 8) * noise;
    col.a *= pow(saturate(1.0f - abs(i.uv.y - 0.5f) * 2.0f), 5);
                //col.a = 0;
                // apply fog
    UNITY_APPLY_FOG(i.fogCoord, col);
    
    
    return col;
}
            ENDCG
        }
    }
}
