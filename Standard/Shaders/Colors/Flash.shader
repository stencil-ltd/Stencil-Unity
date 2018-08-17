Shader "Unlit/Flash"
{
	Properties
	{
		_FlashColor ("Flash Color", Color) = (1, 1, 1)
		_Flash ("Flash", Range(0, 1)) = 0 
	}
	SubShader
	{
        Tags { 
            "Queue" = "Transparent"
            "RenderType" = "Transparent" 
        }        
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
			fixed _Flash;
						
			v2f vert (appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);  
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			    fixed4 tex = 1;
			    tex.rgb = _FlashColor;
				tex.a = lerp(0, 1, _Flash);						
                return tex;
			}
			ENDCG
		}
	}
}
