﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

 Shader "Unlit/GreyScale" {
 Properties {
     _MainTex ("Texture", 2D) = "white" { }
     _Factor ("Factor", Range(0, 1)) = 1
     _Tint ("Tint", Color) = (1,1,1,1)
 }
 SubShader {
     Pass {
 
 CGPROGRAM
 #pragma vertex vert
 #pragma fragment frag
 
 #include "UnityCG.cginc"
 
 sampler2D _MainTex;
 
 struct v2f {
     float4  pos : SV_POSITION;
     float2  uv : TEXCOORD0;
 };
 
 float4 _MainTex_ST;
 float _Factor;
 fixed4 _Tint;
 
 v2f vert (appdata_base v)
 {
     v2f o;
     o.pos = UnityObjectToClipPos (v.vertex);
     o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
     return o;
 }
 
 half4 frag (v2f i) : COLOR
 {
     half4 texcol = tex2D (_MainTex, i.uv);
     texcol.rgb = dot(texcol.rgb, _Tint);
     float3 rgb = dot(texcol.rgb, float3(0.3, 0.59, 0.11));     
     texcol.rgb = lerp(texcol.rgb, rgb, _Factor);
     return texcol;
 }
 ENDCG
 
     }
 }
 Fallback "VertexLit"
 } 