Shader "Unlit/Falloff"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		[MaterialToggle] _ApplyTint("Apply Tint", Int) = 1
		[PerRendererData] _MultColor ("Tint Mult", Color) = (1,1,1,1)	
		[PerRendererData] _AddColor("Tint Add", Color) = (0,0,0,0)
		_Alpha ("Alpha", Range(0, 1)) = 1
		
		[PerRendererData] [MaterialToggle] _DisableAllFades("Disable All Fades", Int) = 0
		[PerRendererData] [MaterialToggle] _UseDistance ("Use Distance", Int) = 0
		[PerRendererData] [MaterialToggle] _UseHeight ("Use Height", Int) = 0
		[PerRendererData] [MaterialToggle] _DistanceDropoff ("Distance Dropoff", Int) = 0
		
		[PerRendererData] _FlashColor ("Flash Color", Color) = (1, 1, 1)
		[PerRendererData] _Flash ("Flash", Range(0, 1)) = 0   		
	}
	SubShader
	{
        Tags
        { 
            "Queue" = "Transparent-100"
            "IgnoreProjector" = "True" 
            "RenderType" = "Transparent" 
        }
        
//        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
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
                float3 worldPos : TEXCOORD1;
//                half3 worldRefl : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _MultColor;
			fixed4 _AddColor;
			int _ApplyTint;
			int _UseDistance;
			int _UseHeight;
			int _DisableAllFades;
			int _DistanceDropoff;
			float _Alpha;
			
			fixed3 _FlashColor;
			fixed _Flash;
			
			// Globals
			float3 _FogPoint;
            half4 _FogColor;
            
            float _FogHeightMin;
            float _FogHeightMax;
            
            float _FogDistMin;
            float _FogDistMax;
            
            float _DropDistMin;
            float _DropDistMax;
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				if (_DistanceDropoff)
				{				
                    fixed4 world = mul (unity_ObjectToWorld, v.vertex);
                    float yScale = length(float3(unity_ObjectToWorld[0].y, unity_ObjectToWorld[1].y, unity_ObjectToWorld[2].y));
			        float d = distance(_FogPoint, world);
                    float norm = (d - _DropDistMin) / (_DropDistMax - _DropDistMin);
                    if (norm < 0) norm = 0;
                    float smooth = 10 * norm * norm;
				    v.vertex.y -= smooth / yScale;
				}
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			    fixed4 tex = tex2D (_MainTex, i.uv);
			    if (_ApplyTint)
			    {
			        tex *= _MultColor;
			        tex += _AddColor;
			    }
			    tex.rgb = lerp(tex.rgb, _FlashColor, _Flash);
				tex.a *= _Alpha;
			
			    fixed4 color = _FogColor;
			    color.a = tex.a;
			    if (_ApplyTint)
			    {
			        color *= _MultColor;
			        color += _AddColor;
			    }
			    float d = 1;
			    float norm = 0;
			    if (!_DisableAllFades && _UseDistance) {
			        d = distance(_FogPoint, i.worldPos);
                    norm = (d - _FogDistMin) / (_FogDistMax - _FogDistMin);
                }
			    if (!_DisableAllFades && _UseHeight) {
			        d = _FogPoint.y - i.worldPos.y;
			        norm = max(norm, (d - _FogHeightMin) / (_FogHeightMax - _FogHeightMin));
                }
                float smooth = smoothstep(0, 1, clamp(norm, 0, 1));
                return lerp(tex, color, smooth);
			}
			ENDCG
		}
	}
}
