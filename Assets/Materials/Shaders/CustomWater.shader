Shader "Custom/Water"
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
		_FoamThicness ("Foam Thiccness",Range(0.0,0.1))=0.05
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

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha vertex:vert
		
        #pragma target 3.0


        struct Input
        {
            float3 worldPos;
			float3 vertex;
			float4 screenPos;
        };

        sampler2D _Noise;
        fixed4 _WaterColor;
        fixed4 _FoamColor;
		float4 _Drift;
		float _Scale;
		float _Wave;
		float _Distortion;
		float _Direction;
		float _Bounce;
		float _FoamThicness;
		float _FoamGradient;
		float _Shine;
		float _Shinyness;
		float _Fadeout;
		sampler2D _CameraDepthTexture;

		void vert(inout appdata_full v) {
            if(_Wave>0){
        		float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
				float2 p=(worldPos.xy-_Drift.zw*_Time.y)/_Scale;
				float wave=cos(p.x*_Direction+p.y*(1-_Direction))*_Distortion+tex2Dlod(_Noise,float4(p.xy/10,0,0)).rgb*(1-_Distortion);
				// v.vertex.xyz+=sin(_Time.z)*v.normal;
				worldPos.z-=wave*_Wave;
				// v.normal*=lerp(float3(0,1,0),float3(1,0,0),(wave+1)/2);
				float3 unvertex=v.vertex;
				v.vertex=mul(unity_WorldToObject,worldPos);
				// v.normal=-normalize(v.vertex-unvertex);
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
        void surf (Input i, inout SurfaceOutputStandard o) {
			float2 value = (i.worldPos.xy-_Drift.xy*_Time.y) / _Scale;
			float3 noise = voronoiNoise(value);
			
			if(noise.z>_FoamThicness+_FoamGradient)o.Albedo=_WaterColor;
			else if(noise.z<_FoamThicness) o.Albedo = _FoamColor;
			else o.Albedo=lerp(_FoamColor,_WaterColor,(noise.z-_FoamThicness)/_FoamGradient).rgb;
			float alb=tex2D(_Noise,value).rgb;
			float mod=tex2D(_Noise,i.worldPos.xy).rgb;
			float f=abs(sin(_Time.y+mod*4))*alb*2-1;
			if(f>1-_Shine)o.Albedo+=f*_Shinyness;
			o.Alpha=_WaterColor.a;
			o.Alpha=saturate((LinearEyeDepth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)).r)-i.screenPos.w)/_Fadeout);
		}
        ENDCG
    }
    FallBack "Diffuse"
}
