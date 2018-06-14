Shader "Custom/TessTest" 
{
	Properties 
	{
		_Tess ("Tessellation", Range(0, 64)) = 4
		_Color ("Color", Color) = (1,1,1,0)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_DispTex ("Displacement Texture", 2D) = "gray" {}
		_Displacement ("Displacement", Range(0, 1.0)) = 0.3
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:disp tessellate:tessFixed nolightmap
		#pragma target 4.6

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
		}

		struct Input 
		{
			float2 uv_MainTex;
		};

		sampler2D _MainTex;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 color = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = color.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}