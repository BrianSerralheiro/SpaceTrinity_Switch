Shader "UI/SelectorShader"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_Thicc("Thiccness",Range(0.1,0.5)) = 0.2
		_Freq("Frequency", float) = 10
		_Alpha("Alpha", Range(0.1,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Transparent"

			"IgnoreProjector" = "True"

			"RenderType" = "Transparent"

			"PreviewType" = "Plane"

			"CanUseSpriteAtlas" = "True"}
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
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
            };

            sampler2D _MainTex;
			float _Freq;
			float _Thicc;
			float _Alpha;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
				o.position = v.vertex;
				o.vertex = UnityObjectToClipPos(o.position);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				half4 col = tex2D(_MainTex, i.uv);
				if (col.a < 1) {
					float f = _Time.y%_Freq;
					if (col.a<f && col.a>f - _Thicc)col.a = _Alpha;
					else col.a = 0;
				}
                return col;
            }
            ENDCG
        }
    }
}
