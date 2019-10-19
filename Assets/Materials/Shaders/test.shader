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

        //Blend One OneMinusSrcAlpha

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

            #include "UnitySprites.cginc"
            

            struct appdata
            {
				float4 vertex : POSITION;
				float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };
            
            struct vf
            {
				float2 uv : TEXCOORD0;
				float4 position : TEXCOORD1;
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
            };
            
            sampler2D _BackgroundTexture;
            vf vert (appdata v)
            {
                vf o;
                o.vertex=v.vertex;
                o.position = ComputeGrabScreenPos(o.vertex);
                float2 f=UnityObjectToViewPos(v.vertex);
                o.position+=float4(f,0,0);
                return o;
            }
            fixed4 frag (vf i) : SV_Target
            {
                fixed4 f=tex2D(_MainTex, i.uv);
                if(f.a<1)
                return tex2D(_BackgroundTexture, i.position);
                return f;
            }

        ENDCG

        }

    }

}