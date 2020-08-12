Shader "Custom/Grass"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _WindNoise("Wind Noise",2D)="white"{}
        _Speed("Wind Speed",float)=1
        _Force("Wind Force",Range(0.1,2))=1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 400
		Blend SrcAlpha OneMinusSrcAlpha
        // ZWrite off

        CGPROGRAM
        #pragma surface surf StandardSpecular fullforwardshadows alpha vertex:vert

        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        sampler2D _WindNoise;
        fixed4 _Color;
        float _Speed;
        float _Force;
		void vert(inout appdata_full v){
            float f=v.vertex.y;
            float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
            worldPos.y-=_Time.y*_Speed;
            float w=tex2Dlod(_WindNoise,float4(worldPos.xy/10,0,0)).rgb;
            v.vertex.z=w*f*_Force;
            // v.vertex.y=f-w*f;
        }
        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb * _Color.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    // FallBack "Cutout"
}
