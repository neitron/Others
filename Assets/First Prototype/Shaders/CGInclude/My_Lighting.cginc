// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#if !defined(MY_LIGHTING_INCLUDED)
#define MY_LIGHTING_INCLUDED

#include "UnityPBSLighting.cginc"
#include "AutoLight.cginc"

float4 _Tint;

sampler2D	_MultiTexMask;

sampler2D	_MainTex, _DetailTex, _DetailMask;
float4		_MainTex_ST, _DetailTex_ST;

sampler2D	_Normal, _DetailNormal;
float		_NormalScale, _DetailNormalScale;

float4	_SpecularTint;

sampler2D _MetallicMap;
float	  _Metallic;

float	_Smoothness;

sampler2D _OcclusionMap;
float _OcclusionStrenght;

sampler2D _EmissionMap;
float3 _Emission;

sampler2D _DetailTex2;
sampler2D _DetailTex3;

struct VertexData
{
	float4 vertex	: POSITION; //	local obj
	float3 normal 	: NORMAL;
	float4 tangent	: TANGENT;
	float2 uv 		: TEXCOORD0;
};

struct Interpolators
{
	float4 pos		: SV_POSITION;
	float4 uv 		: TEXCOORD0;
	float3 normal 	: TEXCOORD1; 
	
	#if defined(BINORMAL_PER_FRAGMENT)
		float4 tangent	: TEXCOORD2;
	#else
		float3 tangent	: TEXCOORD2;
		float3 binormal : TEXCOORD3;
	#endif

	float3 worldPos : TEXCOORD4;

	float2 uvSplat : TEXCOORD5;

	SHADOW_COORDS( 6 ) // float4 shadowCoordinates : TEXCOORD6;

	#if defined(VERTEXLIGHT_ON)
		float3 vertexLightColor : TEXCOORD7;
	#endif
};

float3 GetDetailMask( Interpolators i )
{
#if defined(_DETAIL_MASK)
	return tex2D( _DetailMask, i.uv.xy ).a;
#else
	return 1.0f;
#endif
}

float3 GetOcclusion( Interpolators i )
{
#if defined(_OCCLUSION_MAP)
	return lerp(1.0f, tex2D( _OcclusionMap, i.uv.xy ).g, _OcclusionStrenght);
#else
	return 1.0f;
#endif
}

float3 GetEmission( Interpolators i )
{
#if defined(FORWARD_BASE_PASS)
#if defined(_EMISSION_MAP)
	return tex2D( _EmissionMap, i.uv.xy ) * _Emission;
#else
	return _Emission;
#endif
#else
	return 0;
#endif
}

float GetMetallic( Interpolators i )
{
#if defined(_METALLIC_MAP)
	return tex2D( _MetallicMap, i.uv.xy ).r;
#else
	return _Metallic;
#endif
}

float GetSmoothness( Interpolators i )
{
	float smoothness = 1;
#if defined(_SMOOTHNESS_ALBEDO)
	return tex2D( _MainTex, i.uv.xy ).a;
#elif defined(_SMOOTHNESS_METALLIC) && defined(_METALLIC_MAP)
	return tex2D( _MetallicMap, i.uv.xy ).a;
#endif
	return smoothness * _Smoothness;
}

void ComputeVertexLightColor( VertexData v, inout Interpolators i )
{
	#if defined(VERTEXLIGHT_ON)
		i.vertexLightColor = Shade4PointLights(
			unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
			unity_LightColor[0].rgb, unity_LightColor[1].rgb,
			unity_LightColor[2].rgb, unity_LightColor[3].rgb,
			unity_4LightAtten0, i.worldPos, i.normal
		);
				
	#endif
}

float3 CreateBinormal( float3 normal, float3 tangent, float binormalSign )
{
	return cross( normal, tangent.xyz ) *
		( binormalSign * unity_WorldTransformParams.w );
}

Interpolators MyVertexProgram( VertexData v )
{
	Interpolators i;
	i.pos 	= UnityObjectToClipPos( v.vertex );
	i.uv.xy 		= TRANSFORM_TEX( v.uv, _MainTex );
	i.uv.zw			= TRANSFORM_TEX( v.uv, _DetailTex );
	i.normal 		= UnityObjectToWorldNormal( v.normal );

	#if defined(BINORMAL_PER_FRAGMENT)
		i.tangent = float4( UnityObjectToWorldDir( v.tangent.xyz ), v.tangent.w );
	#else
		i.tangent = UnityObjectToWorldDir( v.tangent.xyz );
		i.binormal = CreateBinormal( i.normal, i.tangent, v.tangent.w );
	#endif
	
	i.worldPos = mul ( unity_ObjectToWorld, v.vertex );
	i.uvSplat = v.uv;
	
	TRANSFER_SHADOW( i ); // i.shadowCoordinates = i.position;

	ComputeVertexLightColor( v, i );
	return i;
}

UnityLight CreateLight( Interpolators i )
{
	UnityLight light;

#if defined(POINT) || defined(POINT_COOKIE) || defined(SPOT)
	light.dir = normalize( _WorldSpaceLightPos0.xyz - i.worldPos );
#else
	light.dir = _WorldSpaceLightPos0.xyz;
#endif

	// 0 : return value
	// 1 : float attenuation = SHADOW_ATTENUATION(i);
	// 2 : i.worldPos
	UNITY_LIGHT_ATTENUATION( attenuation, i, i.worldPos );

	//attenuation *= GetOcclusion( i );
	light.color = _LightColor0.rgb * attenuation;
	light.ndotl = DotClamped( i.normal, light.dir );

	return light;
}

float3 BoxProjection(
	float3 direction, float3 position,
	float4 cubemapPosition, float3 boxMin, float3 boxMax
) {
	#if UNITY_SPECCUBE_BOX_PROJECTION
		UNITY_BRANCH
		if ( cubemapPosition.w > 0 )
		{
			float3 factors = 
				( ( direction > 0 ? boxMax : boxMin ) - position ) / direction;
			float scalar = min( min( factors.x, factors.y ), factors.z );
			direction = direction * scalar + ( position - cubemapPosition );
		}
	#endif
	return direction;
}

UnityIndirect CreateIndirectLight( Interpolators i, float3 viewDir )
{
	UnityIndirect indirectLight;
	indirectLight.diffuse = 0.0f;
	indirectLight.specular = 0.0f;

	#if defined(VERTEXLIGHT_ON)
		indirectLight.diffuse = i.vertexLightColor;
	#endif

	#if defined(FORWARD_BASE_PASS)
		indirectLight.diffuse += max( 0.0f, ShadeSH9( float4 ( i.normal, 1.0f ) ) );
		float3 reflectionDir = reflect( -viewDir, i.normal );

		Unity_GlossyEnvironmentData envData;
		envData.roughness = 1.0f - GetSmoothness(i);
		envData.reflUVW = BoxProjection(
			reflectionDir, i.worldPos,
			unity_SpecCube0_ProbePosition,
			unity_SpecCube0_BoxMin, unity_SpecCube0_BoxMax
		);

		float3 probe0 = Unity_GlossyEnvironment(
			UNITY_PASS_TEXCUBE( unity_SpecCube0 ), unity_SpecCube0_HDR, envData
		);
		
		envData.reflUVW = BoxProjection(
			reflectionDir, i.worldPos,
			unity_SpecCube1_ProbePosition,
			unity_SpecCube1_BoxMin, unity_SpecCube1_BoxMax
		);

		#if UNITY_SPECCUBE_BLENDING
			float interpolator = unity_SpecCube0_BoxMin.w;
			UNITY_BRANCH
			if ( interpolator < 0.99999f )
			{
				float3 probe1 = Unity_GlossyEnvironment(
					UNITY_PASS_TEXCUBE_SAMPLER( unity_SpecCube1, unity_SpecCube0 ),
					unity_SpecCube0_HDR, envData
				);
				indirectLight.specular = lerp( probe1, probe0, interpolator );
			}
			else
			{
				indirectLight.specular = probe0;
			}
		#else
			indirectLight.specular = probe0;
		#endif

		float occlusion = GetOcclusion( i );
		indirectLight.diffuse *= occlusion;
		indirectLight.specular *= occlusion;

	#endif
	return indirectLight;
}

float3 GetTangentSpaceNormal( Interpolators i )
{
	float3 mainN = float3( 0.0f, 0.0f, 1.0f );

	#if defined(_NORMAL_MAP)
		mainN = UnpackScaleNormal( tex2D( _Normal, i.uv.xy ), _NormalScale );
	#endif

	#if defined(_DETAIL_NORMAL_MAP)
		float3 detailN =
			UnpackScaleNormal( tex2D( _DetailNormal, i.uv.zw ), _DetailNormalScale );
		detailN = lerp( float3 ( 0.0f, 0.0f, 1.0f ), detailN, GetDetailMask( i ) );
		mainN = BlendNormals( mainN, detailN );
	#endif

	return mainN;
}

void InitializeFragmentNormal( inout Interpolators i )
{	
	float3 tangentSpaceN = GetTangentSpaceNormal( i );
	
	#if defined(BINORMAL_PER_FRAGMENT)
		float binormal = CreateBinormal( i.normal, i.tangent.xyz, i.tangent.w );
	#else
		float binormal = i.binormal;
	#endif

	i.normal = normalize(
		tangentSpaceN.x * i.tangent +
		tangentSpaceN.y * binormal +
		tangentSpaceN.z * i.normal );
}

float3 GetAlbedo( Interpolators i )
{
	float4 splat = tex2D( _MultiTexMask, i.uvSplat );

	float3 albedo =
		( tex2D( _DetailTex3, i.uv ) * splat.r +
		  tex2D( _DetailTex2, i.uv ) * splat.g +
		  tex2D( _MainTex, i.uv.xy ) * ( 1.0 - splat.r - splat.g ) ) * _Tint.rgb;

	#if defined (_DETAIL_ALBEDO_MAP)
		float3 details = tex2D(_DetailTex, i.uv.zw) * unity_ColorSpaceDouble;
		albedo = lerp( albedo, albedo * details, GetDetailMask(i) );
	#endif

	return albedo;
}

float4 MyFragmentProgram( Interpolators i ) : SV_TARGET
{
	InitializeFragmentNormal( i );
	
	float3 viewDir = normalize( _WorldSpaceCameraPos - i.worldPos );
	
	float3 specularTint;
	float oneMinusReflectivity;
	float3 albedo = DiffuseAndSpecularFromMetallic(
		GetAlbedo(i), 
		GetMetallic(i),
		specularTint,
		oneMinusReflectivity);

	float4 color = UNITY_BRDF_PBS( 
		albedo,								// albedo
		specularTint,						// spec tint color
		oneMinusReflectivity,				// 
		GetSmoothness(i), 
		i.normal, 
		viewDir, 
		CreateLight(i),						// lights 
		CreateIndirectLight(i, viewDir) ); 
	color.rgb += GetEmission( i );
	return color;
}

#endif