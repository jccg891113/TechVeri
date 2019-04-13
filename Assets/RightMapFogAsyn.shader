Shader "Babel/UI/Fog/RightMapAsyn"
{
	Properties
	{
		[PerRendererData] _MainTex ("Texture", 2D) = "white" {}

		_FogTex ("Fog Texture", 2D) = "white" { }
		_SizeInfo ("Fog Texture", Color) = (0,0,0,0)
	}
	SubShader
	{
		Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="true" }

		Pass
		{
			Cull Off 
			ZWrite Off 
			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha
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
				float4 vertex : SV_POSITION;
				//float2 uv : TEXCOORD0;
				float4 uv : TEXCOORD0;
			};
			
			sampler2D _MainTex;
			half4 _MainTex_TexelSize;
			sampler2D _FogTex;
			float4 _SizeInfo;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.uv = v.uv;
				o.uv.xy = v.uv;
				o.uv.zw = v.uv * _SizeInfo.zw + _SizeInfo.xy;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				//fixed4 col = tex2D(_MainTex, i.uv);
				//fixed4 fogCol = tex2D(_FogTex, i.uv);
				fixed4 col = tex2D(_MainTex, i.uv.xy);
				fixed4 fogCol = tex2D(_FogTex, i.uv.zw);
				return fixed4(col.rgb, fogCol.r);
			}
			ENDCG
		}
	}
}
