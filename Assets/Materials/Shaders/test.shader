Shader "Sprites/Test"

{

    Properties

    {

        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}

    }



    SubShader

    {

        Tags

        {

            "Queue"="Transparent"

            "IgnoreProjector"="True"

            "RenderType"="Transparent"

            "PreviewType"="Plane"

            "CanUseSpriteAtlas"="True"

        }



        Cull Off

        Lighting Off

        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        GrabPass
        {

            "_BackgroundTexture"

        }

        Pass

        {

        CGPROGRAM

            #pragma vertex vert

            #pragma fragment frag

            #pragma target 4.0

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
                float4 originInViewSpace = mul(UNITY_MATRIX_MV, float4(0, 0, 0, 1));
                o.position=v.vertex;
				o.vertex = UnityObjectToClipPos(o.position);
				o.uv = TRANSFORM_TEX(v.uv, _BackgroundTexture);
                o.uv.y=1-o.uv.y;
				o.color = v.color;
                o.render=ComputeGrabScreenPos(originInViewSpace);
                o.uv.x=sin(_Time.y);
                o.uv.y=cos(_Time.y);
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                fixed f=tex2D(_MainTex, i.uv).a;
                if(f==1)return tex2Dproj(_BackgroundTexture,i.render)+0.5;
                return fixed4(1,1,1,0);
            }

        ENDCG

        }

    }

}