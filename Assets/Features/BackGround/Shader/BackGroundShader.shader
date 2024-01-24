Shader"Unlit/BackGroundShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Zoom ("Zoom", Range (0.0, 10.0)) = 0.80
    }
    SubShader
    {
Cull Off // カリングは不要

ZTest Always // ZTestは常に通す

ZWrite Off // ZWriteは不要

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

            float _Zoom;
#define iterations 17
#define formuparam 0.53

#define volsteps 20
#define stepsize 0.1


#define tile   0.850
#define speed  0.010 

#define brightness 0.0015
#define darkmatter 0.300
#define distfading 0.730
#define saturation 0.850

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
    fixed4 color = tex2D(_MainTex, i.uv);
    
    //星
    //get coords and direction
    float2 uv = /*fragCoord.xy / iResolution.xy - .5*/i.uv;
    uv.y *= unity_OrthoParams.y / unity_OrthoParams.x;
    float3 dir = float3(uv * _Zoom, 1.);
    float time = /*_Time * speed + */.25;

	//mouse rotation
    float a1 = .5/* + iMouse.x / iResolution.x * 2.*/;
    float a2 = .8/* + iMouse.y / iResolution.y * 2.*/;
    float2x2 rot1 = float2x2(cos(a1), sin(a1), -sin(a1), cos(a1));
    float2x2 rot2 = float2x2(cos(a2), sin(a2), -sin(a2), cos(a2));
    dir.xz = mul(dir.xz, rot1);
    dir.xy = mul(dir.xy, rot2);
    float3 from = float3(1., .5, 0.5);
    from += float3(time * 2., time, -2.);
    from.xz = mul(from.xz, rot1);
    from.xy = mul(from.xy, rot2);
	
	//volumetric rendering
    float s = 0.1, fade = 1.;
    float3 v = 0.0f;
    for (int r = 0; r < volsteps; r++)
    {
        float3 p = from + s * dir * .5;
        p = abs(float3(tile, tile, tile) - fmod(p, float3(tile * 2., tile * 2., tile * 2.))); // tiling fold
        float pa, a = pa = 0.;
        for (int i = 0; i < iterations; i++)
        {
            p = abs(p) / dot(p, p) - formuparam; // the magic formula
            a += abs(length(p) - pa); // absolute sum of average change
            pa = length(p);
        }
        float dm = max(0., darkmatter - a * a * .001); //dark matter
        a *= a * a; // add contrast
        if (r > 6)
            fade *= 1. - dm; // dark matter, don't render near
		//v+=float3(dm,dm*.5,0.);
        v += fade;
        v += float3(s, s * s, s * s * s * s) * a * brightness * fade; // coloring based on distance
        fade *= distfading; // distance fading
        s += stepsize;
    }
    float l = length(v);
    v = lerp(float3(l, l, l), v, saturation); //color adjust
    color = fixed4(v * .01, 1.0f);
    
    return color;
            }

            ENDCG
        }
    }
}
