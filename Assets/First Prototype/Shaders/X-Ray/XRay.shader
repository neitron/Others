Shader "CS/X-Ray"
{
	Properties
	{
		_Tint ("Tint Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		ZTest always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex	: POSITION;
				float3 normal	: NORMAL;
			};

			struct v2f
			{
				float3 normal	: TEXCOORD1;
				float4 vertex	: SV_POSITION;
				float3 viewDir	: TEXCOORD2;
			};
			
			fixed4 _Tint;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = normalize( mul( transpose( unity_WorldToObject ), v.normal ) );
				o.viewDir = normalize( _WorldSpaceCameraPos.xyz - mul( unity_ObjectToWorld, v.vertex ).xyz );
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float light = 1.0f - dot( i.viewDir, i.normal ) * 3.0f;
				return _Tint + fixed4( light, light, light, 0.0f );
			}
			ENDCG
		}
	}
}
