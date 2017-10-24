Shader "Custom/GaussianBlur13"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		blurRadius ("Blur Radius", Range(0.0, 30.0)) = 15.0
		horizontalStep ("Horizontal Step", Range(0.0, 3.0)) = 1.0
		verticalStep ("Vertical Step", Range(0.0, 3.0)) = 1.0
		resolution ("Blur Resolution", Float) = 800.0
	}

	SubShader
	{
		Tags 
		{
			"RenderType" = "Transparent" 
			"Queue" = "Transparent"
		}

		ZWrite Off 
		Cull Off 
		Blend SrcAlpha OneMinusSrcAlpha

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
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color;

				return o;
			}
			
			sampler2D _MainTex;
			float blurRadius;
			float horizontalStep;
			float verticalStep;
			float resolution;

			fixed4 frag(v2f i) : COLOR
			{
				float4 sum = float4(0.0f, 0.0f, 0.0f, 0.0f);
				float2 texCoord = i.uv;

				float blur = blurRadius / resolution;
				
				sum += tex2D(_MainTex, float2(texCoord.x - 6.0f * blur * horizontalStep, texCoord.y - 6.0f * blur * verticalStep)) * 0.0f;
				sum += tex2D(_MainTex, float2(texCoord.x - 5.0f * blur * horizontalStep, texCoord.y - 5.0f * blur * verticalStep)) * 0.000003f;
				sum += tex2D(_MainTex, float2(texCoord.x - 4.0f * blur * horizontalStep, texCoord.y - 4.0f * blur * verticalStep)) * 0.000229f;
				sum += tex2D(_MainTex, float2(texCoord.x - 3.0f * blur * horizontalStep, texCoord.y - 3.0f * blur * verticalStep)) * 0.005977f;
				sum += tex2D(_MainTex, float2(texCoord.x - 2.0f * blur * horizontalStep, texCoord.y - 2.0f * blur * verticalStep)) * 0.060598f;
				sum += tex2D(_MainTex, float2(texCoord.x - 1.0f * blur * horizontalStep, texCoord.y - 1.0f * blur * verticalStep)) * 0.24173f;

				sum += tex2D(_MainTex, float2(texCoord.x, texCoord.y)) * 0.382925f;

				sum += tex2D(_MainTex, float2(texCoord.x + 1.0f * blur * horizontalStep, texCoord.y + 1.0f * blur * verticalStep)) * 0.24173f;
				sum += tex2D(_MainTex, float2(texCoord.x + 2.0f * blur * horizontalStep, texCoord.y + 2.0f * blur * verticalStep)) * 0.060598f;
				sum += tex2D(_MainTex, float2(texCoord.x + 3.0f * blur * horizontalStep, texCoord.y + 3.0f * blur * verticalStep)) * 0.005977f;
				sum += tex2D(_MainTex, float2(texCoord.x + 4.0f * blur * horizontalStep, texCoord.y + 4.0f * blur * verticalStep)) * 0.000229f;
				sum += tex2D(_MainTex, float2(texCoord.x + 5.0f * blur * horizontalStep, texCoord.y + 5.0f * blur * verticalStep)) * 0.000003f;
				sum += tex2D(_MainTex, float2(texCoord.x + 6.0f * blur * horizontalStep, texCoord.y + 6.0f * blur * verticalStep)) * 0.0f;

				return float4(sum.rgb, 1);
			}
			ENDCG
		}
	}
}