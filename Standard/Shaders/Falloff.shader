Shader "Unlit/Falloff"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		[MaterialToggle] _ApplyTint("Apply Tint", Int) = 1
		_MultColor ("Tint Mult", Color) = (1,1,1,1)	
		_AddColor("Tint Add", Color) = (0,0,0,0)
		_FlashColor ("Flash Color", Color) = (1, 1, 1)
		_Flash ("Flash", Range(0, 1)) = 0
		_Alpha ("Alpha", Range(0, 1)) = 1
		[MaterialToggle] _UseDistance ("Use Distance", Int) = 0 
	}
	SubShader
	{
        Tags{ Queue = Transparent RenderType = Transparent }
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
			fixed3 _FlashColor;
			fixed4 _MultColor;
			fixed4 _AddColor;
			fixed _Flash;
			int _ApplyTint;
			int _UseDistance;
			float _Alpha;
			
			// Globals
            float3 _BikePosition;
            half4 _BikeFogColor;
            float _MinBikeFog;
            float _MaxBikeFog;
			
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
				tex.rgb = lerp(tex.rgb, _FlashColor.rgb, _Flash);
				tex.a = _Alpha;
			
//                half4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, i.worldRefl);
//                half3 skyColor = DecodeHDR (skyData, unity_SpecCube0_HDR);
//                fixed4 color = (1,1,1,1);
//                color.rgb = skyColor;
//                color.a = 1;
			
			    fixed4 color = _BikeFogColor;
			    color.a = 0;
			    float d = 0;
			    if (_UseDistance == 1)
			        d = distance(_BikePosition, i.worldPos);
			    else
			        d = _BikePosition.y - i.worldPos.y;    
                float norm = (d - _MinBikeFog) / (_MaxBikeFog - _MinBikeFog);
                float smooth = smoothstep(0, 1, clamp(norm, 0, 1));
                return lerp(tex, color, smooth);
			}
			ENDCG
		}
	}
}
