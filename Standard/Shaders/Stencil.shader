Shader "Unlit/Stencil Standard"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		[MaterialToggle] _ApplyTint("Apply Tint", Int) = 1
		_Alpha ("Alpha", Range(0, 1)) = 1
		
		[MaterialToggle] _ApplyCel("Apply Cel", Int) = 0
		_CelRamp ("Cel Ramp", 2D) = "white" {}
		
		[PerRendererData] _MultColor ("Tint Mult", Color) = (1,1,1,1)	
		[PerRendererData] _AddColor("Tint Add", Color) = (0,0,0,0)
		
		[PerRendererData] _DisableAllFades("Disable All Fades", Int) = 0
		
		[PerRendererData] _FlashColor ("Flash Color", Color) = (1, 1, 1)
		[PerRendererData] _Flash ("Flash", Range(0, 1)) = 0   		
		
		[PerRendererData] _UseHeight ("Use Height", Int) = 0
		[PerRendererData] _HeightMin ("", Float) = 0
		[PerRendererData] _HeightMax ("", Float) = 0
		
		[PerRendererData] _UseDistance ("Use Distance", Int) = 0
		[PerRendererData] _DistMin ("", Float) = 0
		[PerRendererData] _DistMax ("", Float) = 0
		
		[PerRendererData] _DistanceDropoff ("Distance Dropoff", Int) = 0
		[PerRendererData] _DropDistMin ("", Float) = 0
		[PerRendererData] _DropDistMax ("", Float) = 0		
	}
	SubShader
	{
        Tags
        { 
            "Queue" = "Transparent-100"
            "IgnoreProjector" = "True" 
            "RenderType" = "Transparent" 
            "LightMode" = "ForwardBase" // allows shadow rec/cast
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
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
				float3 normal : NORMAL;                
//                half3 worldRefl : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _MultColor;
			fixed4 _AddColor;
			int _ApplyTint;
			int _DisableAllFades;
			float _Alpha;
			
			int _ApplyCel;
			sampler2D _CelRamp;
            
			int _UseHeight;
            float _HeightMin;
            float _HeightMax;
            
			int _UseDistance;
            float _DistMin;
            float _DistMax;
            
			int _DistanceDropoff;
            float _DropDistMin;
            float _DropDistMax;
			
			fixed3 _FlashColor;
			fixed _Flash;
			
			// Globals
			float3 _FogPoint;
            half4 _FogColor;
			float4 _LightColor0; // provided by Unity
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				if (_DistanceDropoff)
				{				
                    fixed4 world = mul (unity_ObjectToWorld, v.vertex);
			        float d = distance(_FogPoint, world);
                    float norm = (d - _DropDistMin) / (_DropDistMax - _DropDistMin);
                    if (norm < 0) norm = 0;
                    float smooth = 10 * norm * norm;
                    world.y -= smooth;
                    v.vertex = mul (unity_WorldToObject, world);
				}
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
                
                float4 normal4 = float4(v.normal, 0.0); // need float4 to mult with 4x4 matrix
                o.normal = normalize(mul(normal4, unity_WorldToObject).xyz);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			    fixed4 tex = tex2D (_MainTex, i.uv);
			    if (_ApplyCel)
			    {
                    // convert light direction to world space & normalize
                    // _WorldSpaceLightPos0 provided by Unity
                    float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
    
                    // finds location on ramp texture that we should sample
                    // based on angle between surface normal and light direction
                    float ramp = clamp(dot(i.normal, lightDir), 0.001, 1.0);
                    float3 lighting = tex2D(_CelRamp, float2(ramp, 0.5)).rgb;
    
                    // sample texture for color
                    float4 albedo = tex2D(_MainTex, i.uv.xy);
    
                    // _LightColor0 provided by Unity
                    float3 rgb = albedo.rgb * _LightColor0.rgb * lighting;
                    tex = fixed4(rgb, 1.0);
			    }
			    
			    
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
                    norm = (d - _DistMin) / (_DistMax - _DistMin);
                }
			    if (!_DisableAllFades && _UseHeight) {
			        d = _FogPoint.y - i.worldPos.y;
			        norm = max(norm, (d - _HeightMin) / (_HeightMax - _HeightMin));
                }
                float smooth = smoothstep(0, 1, clamp(norm, 0, 1));
                return lerp(tex, color, smooth);
			}
			ENDCG
		}
	}
}
