Shader "Custom/SandBank"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Floor ("Floor",float) = -2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert

        #pragma target 3.0


        struct Input
        {
            float2 uv_MainTex;
        };

        float _Floor;
        sampler2D _MainTex;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full v) {
            float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
            float d=distance(v.vertex.xz,float2(0,0))/5;
            worldPos.z=d*_Floor+(1-d)*worldPos.z;
            v.vertex=mul(unity_WorldToObject,worldPos);
        }
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
