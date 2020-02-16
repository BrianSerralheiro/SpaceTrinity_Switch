Shader "UI/Hologram"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color",Color)=(0.5,0.5,0.8)
		_Thicc("Thiccness",Range(0.1,0.5)) = 0.2
		_Freq("Frequency", float) = 10
		_Alpha("Alpha", Range(0.1,1)) = 0.5
		
        _StencilComp ("Stencil Comparison", Float) = 8

        _Stencil ("Stencil ID", Float) = 0

        _StencilOp ("Stencil Operation", Float) = 0

        _StencilWriteMask ("Stencil Write Mask", Float) = 255

        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags { "Queue" = "Transparent"

			"IgnoreProjector" = "True"

			"RenderType" = "Transparent"

			"PreviewType" = "Plane"

			"CanUseSpriteAtlas" = "True"}
			Stencil

        {

            Ref [_Stencil]

            Comp [_StencilComp]

            Pass [_StencilOp]

            ReadMask [_StencilReadMask]

            WriteMask [_StencilWriteMask]

        }
		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma target 2.0
			#include "UnityCG.cginc"
			#include "UnityUI.cginc"
			#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
			#pragma multi_compile_local _ UNITY_UI_ALPHACLIP
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
				float2 screen:TEXCOORD2;
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
            };

            sampler2D _MainTex;
			float4 _Color;
			float _Freq;
			float _Thicc;
			float _Alpha;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
				o.position = v.vertex;
				o.vertex = UnityObjectToClipPos(o.position);
				o.screen = ComputeScreenPos(o.position).xy/_ScreenParams.xy;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				half4 col = tex2D(_MainTex, i.uv);
				col.a*=i.color.a;
				i.screen.y+=1;
				i.screen.y/=2;
				if (col.a > 0) {
					float f = _Time.y / 6 %_Freq;
					if (i.screen.y<f && i.screen.y>f - _Thicc/2){
						float a=_Alpha+floor(i.screen.y%(_Thicc/10)/(_Thicc/10)+0.5)*(1-_Alpha);
						col.rgb*=1-a;
						col.rgb += _Color.rgb*a;
					}
				}
				//col.rgb /= 2;
				return col;
            }
            ENDCG
        }
    }
}
