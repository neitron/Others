Shader "CS/XRay _Replacement"
{
	Properties
	{ }
	SubShader
	{
		Tags 
		{ 
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
			"XRay" = "ColorOutline"
		}
		ZTest Always
		ZWrite Off
		Blend One One
		

		Pass
		{
			Stencil
			{
				Ref 1		  // working value
				Comp NotEqual // draw if stencil value not equal to '1'
				Pass Keep 	  // keep the same value
			}
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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				float3x3 m = transpose(unity_WorldToObject);
				o.normal = normalize( mul( m, v.normal ) );
				o.viewDir = normalize( _WorldSpaceCameraPos.xyz - mul( unity_ObjectToWorld, v.vertex ).xyz );
				return o;
			}

			fixed4 _XRayEdgeColor;
			float _GlobalXRayVisibility;
			
			fixed4 frag (v2f i) : SV_Target
			{
				float light = 1.0f - dot( i.viewDir, i.normal ) * 3.0f;
				return _XRayEdgeColor * light * _GlobalXRayVisibility;
			}
			ENDCG
		}
	}
}
