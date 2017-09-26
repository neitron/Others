Shader "CS/LightingAndXRay" 
{
	Properties
	{
		_Tint( "Tint", Color ) = ( 1.0, 1.0, 1.0, 1.0 )

		_MainTex( "Texture", 2D ) = "white" {}

		_XRayEdgeColor( "X-Ray Edge Color", Color ) = ( 1.0, 0.5, 0.4, 1.0 )

		[NoScaleOffset] _Normal( "Normal", 2D ) = "bump" {}
		_NormalScale( "Normal Scale", Float ) = 1.0

		[Space( 25 )]
		_SpecularTint( "Specular", Color ) = ( 0.5, 0.5, 0.5 )

		[NoScaleOffset] _MetallicMap( "Metallic map", 2D ) = "white" {}
		[Gamma] _Metallic( "Metallic", Range( 0, 1 ) ) = 0
		_Smoothness( "Smothness", Range( 0.0, 1.0 ) ) = 0.1

		[NoScaleOffset] _OcclusionMap( "Occlusion", 2D ) = "white" {}
		_OcclusionStrenght( "Occlusion Strenght", Range(0.0, 1.0) ) = 1.0

		[NoScaleOffset] _EmissionMap( "Emission", 2D ) = "black" {}
		_Emission( "Emission", Color ) = ( 0, 0, 0 )

		[NoScaleOffset] _DetailMask( "Detail Mask", 2D ) = "white" {}

		[Space( 25 )]
		_DetailTex( "Detail Texture", 2D ) = "gray" {}
		[NoScaleOffset] _DetailNormal( "Detail Normal", 2D ) = "bump" {}
		_DetailNormalScale( "Normal Scale", Float ) = 1.0

		[Space( 25 )]
		[NoScaleOffset]_MultiTexMask( "Multi-texture mask", 2D ) = "black" {}
		[NoScaleOffset] _DetailTex2( "Texture 2", 2D ) = "white" {}
		[NoScaleOffset] _DetailTex3( "Texture 3", 2D ) = "white" {}
	}

		CGINCLUDE
	#define BINORMAL_PER_FRAGMENT
		ENDCG

	SubShader
	{			 
		Tags 
		{ 
			"XRay" = "ColorOutline"
		}
		Pass
		{
			Tags
			{
				"LightMode" = "ForwardBase"
			}
			Stencil
			{
				Ref 1		 // working value 
				Comp Always  // always draw (ignore compression)
				Pass Replace // write '1' to stencil
			}
			CGPROGRAM

				#pragma target 3.0

				#pragma shader_feature _METALLIC_MAP
				#pragma shader_feature _ _SMOOTHNESS_ALBEDO _SMOOTHNESS_METALLIC
				#pragma shader_feature _NORMAL_MAP
				#pragma shader_feature _OCCLUSION_MAP
				#pragma shader_feature _EMISSION_MAP
				#pragma shader_feature _DETAIL_MASK
				#pragma shader_feature _DETAIL_ALBEDO_MAP
				#pragma shader_feature _DETAIL_NORMAL_MAP

				#pragma multi_compile _ SHADOWS_SCREEN
				#pragma multi_compile _ VERTEXLIGHT_ON

				#pragma vertex MyVertexProgram
				#pragma fragment MyFragmentProgram

				#define FORWARD_BASE_PASS

				#include "../CGInclude/My_Lighting.cginc"	

			ENDCG
		}

		Pass
		{
			Tags
			{
				"LightMode" = "ForwardAdd"
			}

			Blend one one
			ZWrite off

			CGPROGRAM
				#pragma target 3.0

				#pragma shader_feature _METALLIC_MAP
				#pragma shader_feature _ _SMOOTHNESS_ALBEDO _SMOOTHNESS_METALLIC
				#pragma shader_feature _NORMAL_MAP
				#pragma shader_feature _DETAIL_MASK
				#pragma shader_feature _DETAIL_ALBEDO_MAP
				#pragma shader_feature _DETAIL_NORMAL_MAP
				#pragma multi_compile_fwdadd_fullshadows

				#pragma vertex MyVertexProgram
				#pragma fragment MyFragmentProgram

				#include "../CGInclude/My_Lighting.cginc"	

			ENDCG
		}

		Pass
		{
			Tags
			{
				"LightMode" = "ShadowCaster"
			}

			CGPROGRAM
				#pragma target 3.0

				#pragma multi_compile_shadowcaster

				#pragma vertex MyShadowVertexProgram
				#pragma fragment MyShadowFragmentProgram

				#include "../CGInclude/My_Shadows.cginc"	

			ENDCG
		}
	}

		CustomEditor "LightingAndXRayShaderGUI"
}

