﻿Shader "Custom/Water"
{
    Properties
    {
        _WaterColor ("Water Color", Color) = (1,1,1,1)
        _FoamColor ("Foam Color", Color) = (1,1,1,1)
		_Wave ("Wave Heigth",float)=1
		_Fadeout ("Fadeout",float)=1
		_Bounce ("Bounce",float)=1
		_Distortion("Distortion",Range(0,1))=0
		_Direction("Direction",Range(0,1))=0
		_Reflection("Reflection",Range(0,1))=0.5
		_FoamThicness ("Foam Thiccness",Range(0.0,1))=0.05
		_FoamGradient ("Foam Gradient",Range(0.0,0.4))=0.1
		_Drift ("Drift , Wave Speed",Vector)=(0,0,0,0)
		_Scale ("Scale",float)=2
		_Shinyness("Shinyness",Range(0,0.3))=0.01
		_Shine ("Shine",Range(0,1))=0.5
        _Noise ("Water Noise", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 200
		// Zwrite off
		Blend SrcAlpha OneMinusSrcAlpha
		GrabPass
        {
            "_BackgroundTexture"
        }
        CGPROGRAM
        #pragma surface surf StandardSpecular fullforwardshadows alpha vertex:vert
		
        #pragma target 3.0


        struct Input
        {
            float3 worldPos;
			float4 vertex;
			float4 screenPos;
        };

        sampler2D _BackgroundTexture;
        sampler2D _Noise;
        fixed4 _WaterColor;
        fixed4 _FoamColor;
		float4 _Drift;
		float _Scale;
		float _Wave;
		float _Reflection;
		float _Distortion;
		float _Direction;
		float _Bounce;
		float _FoamThicness;
		float _FoamGradient;
		float _Shine;
		float _Shinyness;
		float _Fadeout;
		sampler2D _CameraDepthTexture;

		float waveSample(float2 p){
			return cos(p.x*_Direction+p.y*(1-_Direction))*_Distortion+tex2Dlod(_Noise,float4(p.xy/10,0,0)).rgb*(1-_Distortion);
		}
		float calculateNormal(float wave,float2 pos,float2 mod){
			float past=waveSample(pos-mod);
			float future=waveSample(pos+mod);
			float dif1=future-wave;
			float dif2=wave-past;
			return (dif1+dif2)*5;
		}
		void vert(inout appdata_full v){
            if(_Wave>0){
        		float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
				float2 p=(worldPos.xy-_Drift.zw*_Time.y)/_Scale;
				float mod=1/_Scale;
				float bounce=0;//cos(_Time.y*_Bounce)/2+0.5;
				float wave=waveSample(p)*bounce+waveSample(ComputeScreenPos(v.vertex).xy-_Drift.zw*_Time.y)*(1-bounce);
				// v.vertex.xyz+=sin(_Time.z)*v.normal;
				worldPos.z-=wave*_Wave;
				// v.normal*=lerp(float3(0,1,0),float3(1,0,0),(wave+1)/2);
				v.vertex=mul(unity_WorldToObject,worldPos);
				v.normal=normalize(float3(calculateNormal(wave,p,float2(mod,0)),1,-calculateNormal(wave,p,float2(0,mod))));
			}
        }
        float3 voronoiNoise(float2 value){
			float2 baseCell = floor(value);

			float minDistToCell = 10;
			float2 toClosestCell;
			float2 closestCell;
			[unroll]
			for(int x1=-1; x1<=1; x1++){
				[unroll]
				for(int y1=-1; y1<=1; y1++){
					float2 cell = baseCell + float2(x1, y1);
					float2 cellPosition = cell+((sin(_Time.y*_Bounce)/4+1)*tex2D(_Noise,cell/10).rgb);
					float2 toCell = cellPosition - value;
					float distToCell = length(toCell);
					if(distToCell < minDistToCell){
						minDistToCell = distToCell;
						closestCell = cell;
						toClosestCell = toCell;
					}
				}
			}

			//second pass to find the distance to the closest edge
			float minEdgeDistance = 10;
			[unroll]
			for(int x2=-1; x2<=1; x2++){
				[unroll]
				for(int y2=-1; y2<=1; y2++){
					float2 cell = baseCell + float2(x2, y2);
					float2 cellPosition = cell+((sin(_Time.y*_Bounce)/4+1)*tex2D(_Noise,cell/10).rgb);
					float2 toCell = cellPosition - value;

					float2 diffToClosestCell = abs(closestCell - cell);
					bool isClosestCell = diffToClosestCell.x + diffToClosestCell.y < 0.1;
					if(!isClosestCell){
						float2 toCenter = (toClosestCell + toCell) * 0.5;
						float2 cellDifference = normalize(toCell - toClosestCell);
						float edgeDistance = dot(toCenter, cellDifference);
						minEdgeDistance = min(minEdgeDistance, edgeDistance);
					}
				}
			}

			float random = closestCell*0.1;
    		return float3(minDistToCell, random, minEdgeDistance);
		}
        void surf (Input i, inout SurfaceOutputStandardSpecular o) {
			float2 value = (i.worldPos.xy-_Drift.xy*_Time.y) / _Scale;
			float bounce=cos(_Time.y*_Bounce)/2+0.5;
			float noise =waveSample((i.worldPos.xy-_Drift.zw*_Time.y)/_Scale)*bounce+waveSample(i.screenPos.xy-_Drift.zw*_Time.y)*(1-bounce);
			
			// if(1-noise<_FoamThicness)o.Albedo=_FoamColor;
			// else if(noise>1-_FoamThicness) o.Albedo = _FoamColor;
			o.Albedo=lerp(_WaterColor,_FoamColor,clamp(_FoamThicness*noise*2-1,0,1));
			// float dist=LinearEyeDepth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)).r)-i.screenPos.w;
			float depth=saturate((LinearEyeDepth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)).r)-i.screenPos.w)/_Fadeout);
			float2 uv=i.screenPos.xy/i.screenPos.w;
			float2 p=(i.worldPos.xy-_Drift.zw*_Time.y/5+float2(uv.x-0.5,uv.y-0.5)*-10)/_Scale;
			float wave=tex2D(_Noise,p).rgb;
			float4 scrmod=float4(wave*-(uv.x-0.5)*2,wave*-(uv.y-0.5)*2,0,0);
			float dist=LinearEyeDepth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos+scrmod)).r)-i.screenPos.w;
			float2 uvpos=(i.screenPos.xy+scrmod.xy*clamp(abs(dist),0,1))/i.screenPos.w;
			uvpos.y=1-uvpos.y;
			// uvpos+=float2(uvpos.x/2+0.5,uvpos.y/2+0.5);
			if(dist<0)o.Albedo=o.Albedo*(1-_Reflection)+tex2D(_BackgroundTexture,uvpos).rgb*_Reflection;
			// lerp(o.Albedo,tex2D(_BackgroundTexture,i.screenPos.xy+scrmod).rgb,_Reflection*depth);
			float alb=(tex2D(_Noise,value).rgb*2-1)/10+1;
			float mod=tex2D(_Noise,(i.worldPos.xy-_Drift.xy*_Time.y)/_Scale).rgb;
			float f=alb*noise;
			if(f>_Shine)o.Specular=f*_Shinyness;
			o.Alpha=depth;
			// o.Emission=f*_Shinyness;
		}
        ENDCG
    }
    // FallBack "Diffuse"
}
