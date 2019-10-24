// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Sprites/GravShader"

{
    Properties

    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "Queue"="Overlay"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
        Cull Off
        Lighting Off
        ZWrite Off
        //ZTest Always
        //Blend SrcAlpha OneMinusSrcAlpha

        GrabPass
        {
            "_BackgroundTexture"
        }
        Pass
        {
        CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
            
            struct appdata
            {
				float4 vertex : POSITION;
				float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
				float2 uv : TEXCOORD0;
				float4 position : TEXCOORD1;
				float4 render : TEXCOORD2;
                float4 origin:TEXCOORD3;
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
            };
            
            sampler2D _BackgroundTexture;
            float4 _BackgroundTexture_ST;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            v2f vert (appdata v)
            {
                v2f o;
                /*float4 originInViewSpace = mul(UNITY_MATRIX_MV, float4(0, 0, 0, 1));
                o.position=v.vertex;
				o.vertex = UnityObjectToClipPos(o.position);
				o.uv = TRANSFORM_TEX(v.uv, _BackgroundTexture);
                o.uv.y=1-o.uv.y;
				o.color = v.color;
                o.render=ComputeGrabScreenPos(originInViewSpace);
                o.uv.x=sin(_Time.y);
                o.uv.y=cos(_Time.y);
                return o;*/
                o.uv=v.uv;
                o.vertex= UnityObjectToClipPos(v.vertex);
                o.render=ComputeScreenPos(o.vertex);
                o.render.y=1-o.render.y;
                /*float4 origin=mul(unity_ObjectToWorld, float4(0.0,0.0,0.0,1.0));
                float4 scrPos=ComputeScreenPos(UnityObjectToClipPos(v.vertex));
                float4 scrOri=ComputeScreenPos(UnityWorldToClipPos(origin));
                scrPos.xy/=scrPos.w;
                scrOri.xy/=scrOri.w;
                float f=distance(scrPos,scrOri);
                float2 f1=float2(f/0.15,f/0.2);
                f1.y+=abs(scrPos.x-scrOri.x)*(scrOri.y-_ScreenParams.w);
                float2 f2=o.render.xy-scrOri;
                //o.render.xy+=-f2+f2/f1;
                float w=abs(scrOri.x-scrPos.x);
                float h=abs(scrOri.y-scrPos.y);
                o.render.x+=w;*/
                //o.render.y+=h/5;
                float deltaX=abs(v.vertex.x)/3;
                float deltaY=abs(v.vertex.y)/3;
                float4 scrPos=ComputeScreenPos(UnityObjectToClipPos(v.vertex));
                float4 scrOri=ComputeScreenPos(UnityObjectToClipPos(float4(0,0,0,1)));
                float w=scrPos.x-scrOri.x;
                float h=scrPos.y-scrOri.y;
                o.render.x-=+w-w*(deltaX);
                o.render.y+=-h+h*(deltaY*2);
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                //return i.render.x;
                fixed4 f1=tex2D(_MainTex,i.uv);
                fixed4 f2=tex2D(_BackgroundTexture, i.render.xy/i.render.w);
                float f=f1.a;
                return f1*f+f2*(1-f);
            }
        ENDCG
        }
    }

}