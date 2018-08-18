Shader "Unlit/Falloff"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		[MaterialToggle] _ApplyTint("Apply Tint", Int) = 1
		_MultColor ("Tint Mult", Color) = (1,1,1,1)	
		_AddColor("Tint Add", Color) = (0,0,0,0)
		_Alpha ("Alpha", Range(0, 1)) = 1
		
		[MaterialToggle] _UseDistance ("Use Distance", Int) = 0
		[MaterialToggle] _UseHeight ("Use Height", Int) = 0  
	}
	SubShader
	{
        Tags{ Queue = Geometry RenderType = Transparent }
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
			float _Alpha;
			
			// Globals
            float3 _FogPoint;
            half4 _FogColor;
            
            float _FogHeightMin;
            float _FogHeightMax;
            
            float _FogDistMin;
            float _FogDistMax;
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);  
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
//                float3 worldViewDir = normalize(UnityWorldSpaceViewDir(o.worldPos));
//                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
//                o.worldRefl = reflect(-worldViewDir, worldNormal);
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
				tex.a = _Alpha;
			
			    fixed4 color = _FogColor;
			    color.a = 0;
			    float d = 1;
			    float norm = 0;
			    if (_UseDistance) {
			        d = distance(_FogPoint, i.worldPos);
                    norm = (d - _FogDistMin) / (_FogDistMax - _FogDistMin);
                }
			    if (_UseHeight) {
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
