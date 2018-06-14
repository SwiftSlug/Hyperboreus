Shader "Custom/TessTest" 
{
	Properties 
	{
		_Tess ("Tessellation", Range(0, 64)) = 4
		_MainColor ("Main Color", Color) = (1, 1, 1, 1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_SubColor ("Sub Color", Color) = (1, 1, 1, 1)
		_SubTex("Sub Texture", 2D) = "white" {}
		_DispTex ("Displacement Texture", 2D) = "black" {}
		_Displacement ("Displacement", Range(0, 2.0)) = 0.3
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:disp tessellate:tessFixed nolightmap
		#pragma target 4.6

		#include "Tessellation.cginc"

		struct appdata 
		{
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
		};

		float _Tess;

		float4 tessFixed()
		{
			return _Tess;
		}

		sampler2D _DispTex;
		float _Displacement;

		void disp(inout appdata v)
		{
			float calcDisplacement = tex2Dlod(_DispTex, float4(v.texcoord.xy, 0, 0)).r * _Displacement;
			v.vertex.xyz -= v.normal * calcDisplacement;
			v.vertex.xyz += v.normal * _Displacement;
		}

		sampler2D _MainTex;
		fixed4 _MainColor;
		sampler2D _SubTex;
		fixed4 _SubColor;

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_SubTex;
			float2 uv_DispTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			half lerpAmount = tex2Dlod(_DispTex, float4(IN.uv_DispTex, 0, 0)).r;
			fixed4 color = lerp(tex2D(_MainTex, IN.uv_MainTex) * _MainColor, tex2D(_SubTex, IN.uv_SubTex) * _SubColor, pow(lerpAmount, 1));
			o.Albedo = color.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}