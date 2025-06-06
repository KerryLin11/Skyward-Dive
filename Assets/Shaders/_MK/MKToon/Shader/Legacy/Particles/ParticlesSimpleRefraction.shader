﻿//////////////////////////////////////////////////////
// MK Toon Built-in Particles Simple + Refraction	//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////

Shader "MK/Toon/Built-in/Particles/Simple + Refraction"
{
	Properties
	{
		/////////////////
		// Options     //
		/////////////////
		[Enum(MK.Toon.SurfaceRefraction)] _Surface ("", int) = 1
		_Blend ("", int) = 0
		[Toggle] _AlphaClipping ("", int) = 0
		[Enum(MK.Toon.RenderFace)] _RenderFace ("", int) = 2

		/////////////////
		// Input       //
		/////////////////
		[MainColor] _AlbedoColor ("", Color) = (1,1,1,1)
		_AlphaCutoff ("", Range(0, 1)) = 0.5
		[MainTexture] _AlbedoMap ("", 2D) = "white" {}
		_AlbedoMapIntensity ("", Range(0, 1)) = 1.0
		[MKToonColorRGB] _SpecularColor ("", Color) = (1, 1, 1, 1)
		_SpecularMap ("", 2D) = "white" {}
		_Smoothness ("", Range(0, 1)) = 0.5
		_NormalMapIntensity ("", Float) = 1
		[Normal] _NormalMap ("", 2D) = "bump" {}
		_EmissionColor ("", Color) = (0, 0, 0, 1)
		_EmissionMap ("", 2D) = "black" {}

		/////////////////
		// Stylize     //
		/////////////////
		[Enum(MK.Toon.Light)] _Light ("", int) = 0
		_DiffuseRamp ("", 2D) = "grey" {}
		_DiffuseSmoothness ("", Range (0.0, 1.0)) = 0.0
		_DiffuseThresholdOffset ("", Range (0.0, 1.0)) = 0.25
		_SpecularRamp("", 2D) = "grey" {}
		_SpecularSmoothness ("", Range (0.0, 1.0)) = 0.0
		_SpecularThresholdOffset ("", Range (0.0, 1.0)) = 0.25
		_RimRamp ("", 2D) = "grey" {}
		_RimSmoothness ("", Range (0.0, 1.0)) = 0.5
		_RimThresholdOffset ("", Range (0.0, 1.0)) = 0.25
		
		[MKToonLightBands] _LightBands ("", Range (2, 12)) = 4
		_LightBandsScale ("", Range (0.0, 1.0)) = 0.5
		_LightThreshold ("", Range (0.0, 1.0)) = 0.5
		_ThresholdMap ("", 2D) = "gray" {}
		_ThresholdMapScale ("", Float) = 1
		_GoochRampIntensity ("", Range (0.0, 1.0)) = 0.5
		_GoochRamp ("", 2D) = "white" {}
		_GoochBrightColor ("", Color) = (1, 1, 1, 1)
		_GoochDarkColor ("", Color) = (0, 0, 0, 1)
		_Contrast ("", Float) = 1.0
		[MKToonSaturation] _Saturation ("", Float) = 1.0
		[MKToonBrightness] _Brightness ("",  Float) = 1
		[Enum(MK.Toon.Iridescence)] _Iridescence ("", int) = 0
		_IridescenceRamp ("", 2D) = "white" {}
		_IridescenceSize ("", Range(0.0, 5.0)) = 1.0
		_IridescenceColor ("", Color) = (1, 1, 1, 0.5)
		_IridescenceSmoothness ("", Range (0.0, 1.0)) = 0.5
		_IridescenceThresholdOffset ("", Range (0.0, 1.0)) = 0.0
		[Enum(MK.Toon.Rim)] _Rim ("", int) = 0
		_RimSize ("", Range(0.0, 5.0)) = 1.0
		_RimColor ("", Color) = (1, 1, 1, 1)
		_RimBrightColor ("", Color) = (1, 1, 1, 1)
		_RimDarkColor ("", Color) = (0, 0, 0, 1)
		[Enum(MK.Toon.ColorGrading)] _ColorGrading ("", int) = 0
		[Toggle] _VertexAnimationStutter ("", int) = 0
		[Enum(MK.Toon.VertexAnimation)] _VertexAnimation ("", int) = 0
        _VertexAnimationIntensity ("", Range(0, 1)) = 0.05
		_VertexAnimationMap ("", 2D) = "white" {}
		_NoiseMap ("", 2D) = "white" {}
        [MKToonVertexAnimationFrequency] _VertexAnimationFrequency ("", Vector) = (2.5, 2.5, 2.5, 1)
		[Enum(MK.Toon.Dissolve)] _Dissolve ("", int) = 0
		_DissolveMapScale ("", Float) = 1
		_DissolveMap ("", 2D) = "white" {}
		_DissolveAmount ("", Range(0.0, 1.0)) = 0.5
		_DissolveBorderSize ("", Range(0.0, 1.0)) = 0.25
		_DissolveBorderRamp ("", 2D) = "white" {}
		[HDR] _DissolveBorderColor ("", Color) = (1, 1, 1, 1)
		[Enum(MK.Toon.Artistic)] _Artistic ("", int) = 0
		[Enum(MK.Toon.ArtisticProjection)] _ArtisticProjection ("", int) = 0
		_ArtisticFrequency ("", Range(1, 10)) = 1
		_DrawnMapScale ("", Float) = 1
		_DrawnMap ("", 2D) = "white" {}
		_DrawnClampMin ("", Range(0.0, 1.0)) = 0.0
		_DrawnClampMax ("", Range(0.0, 1.0)) = 1.0
		_HatchingMapScale ("", Float) = 1
		_HatchingBrightMap ("", 2D) = "white" {}
		_HatchingDarkMap ("", 2D) = "Black" {}
		_SketchMapScale ("", Float) = 1
		_SketchMap ("", 2D) = "black" {}

		/////////////////
		// Advanced    //
		/////////////////
		[HideInInspector] [Enum(MK.Toon.BlendFactor)] _BlendSrc ("", int) = 1
		[HideInInspector] [Enum(MK.Toon.BlendFactor)] _BlendDst ("", int) = 0
		[HideInInspector] [Enum(MK.Toon.BlendFactor)] _BlendSrcAlpha ("", int) = 1
		[HideInInspector] [Enum(MK.Toon.BlendFactor)] _BlendDstAlpha ("", int) = 0
		[Enum(MK.Toon.ZWrite)] _ZWrite ("", int) = 0
		[Enum(MK.Toon.ZTest)] _ZTest ("", int) = 4.0
		[Toggle] _WrappedLighting ("", int) = 1
		_IndirectFade ("", Range(0.0, 1.0)) = 1.0
		[Toggle] _ReceiveShadows("", Int) = 1
		[Enum(MK.Toon.SpecularSimple)] _Specular ("", int) = 0
		[MKToonSpecularIntensity] _SpecularIntensity ("", Float) = 1.0
		[Enum(MK.Toon.EnvironmentReflectionSimple)] _EnvironmentReflections ("", int) = 0
		[MKToonRenderPriority] _RenderPriority ("", Range(-50, 50)) = 0.0

		[Enum(MK.Toon.Stencil)] _Stencil ("", Int) = 1
		[MKToonStencilRef] _StencilRef ("", Range(0, 255)) = 0
		[MKToonStencilReadMask] _StencilReadMask ("", Range(0, 255)) = 255
		[MKToonStencilWriteMask] _StencilWriteMask ("", Range(0, 255)) = 255
		[Enum(MK.Toon.StencilComparison)] _StencilComp ("", Int) = 8
		[Enum(MK.Toon.StencilOperation)] _StencilPass ("", Int) = 0
		[Enum(MK.Toon.StencilOperation)] _StencilFail ("", Int) = 0
		[Enum(MK.Toon.StencilOperation)] _StencilZFail ("", Int) = 0
		
		/////////////////
		// Particles   //
		/////////////////
		[Toggle] _Flipbook ("", Float) = 0.0
		[Toggle] _SoftFade ("", Float) = 0.0
		[MKToonSoftFadeNearDistance] _SoftFadeNearDistance ("", Float) = 0.0
        [MKToonSoftFadeFarDistance] _SoftFadeFarDistance ("", Float) = 1.0
		[Enum(MK.Toon.ColorBlend)] _ColorBlend ("", Float) = 0.0
		[Toggle] _CameraFade ("", Float) = 0.0
        [MKToonCameraFadeNearDistance] _CameraFadeNearDistance ("", Float) = 1.0
        [MKToonCameraFadeFarDistance] _CameraFadeFarDistance ("", Float) = 2.0

		/////////////////
		// Refraction  //
		/////////////////
		_RefractionDistortionMapScale ("", Float) = 1.0
		[Normal] _RefractionDistortionMap ("", 2D) = "bump" {}
		_RefractionDistortion ("", Float) = 0.1
		_IndexOfRefraction ("", Range(0, 0.5)) = 0.0
		_RefractionDistortionFade ("", Range(0.0, 1.0)) = 0.5

		/////////////////
		// Editor Only //
		/////////////////
		[HideInInspector] _Initialized ("", int) = 0
        [HideInInspector] _OptionsTab ("", int) = 1
		[HideInInspector] _InputTab ("", int) = 1
		[HideInInspector] _StylizeTab ("", int) = 0
		[HideInInspector] _AdvancedTab ("", int) = 0
		[HideInInspector] _ParticlesTab ("", int) = 0
		[HideInInspector] _RefractionTab ("", int) = 0

		/////////////////
		// System	   //
		/////////////////
		[HideInInspector] _Cutoff ("", Range(0, 1)) = 0.5
		[HideInInspector] _MainTex ("", 2D) = "white" {}
		[HideInInspector] _Color ("", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags {"RenderType"="Transparent" "PerformanceChecks"="False" "IgnoreProjector" = "True" "PreviewType" = "Plane"}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// REFRACTION
		/////////////////////////////////////////////////////////////////////////////////////////////
		GrabPass { Tags { "LightMode" = "Always" } "_MKToonRefraction" Name "MKSharedRefraction" }

		/////////////////////////////////////////////////////////////////////////////////////////////
		// Editor Only
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Name "SceneSelectionPass"
			Tags { "LightMode" = "SceneSelectionPass" }

			BlendOp Add
			Blend One Zero
			ZWrite On
			Cull Off

			CGPROGRAM
			#pragma target 4.5
			#pragma exclude_renderers gles gles3 glcore d3d11_9x wiiu n3ds switch

			#pragma multi_compile_instancing
			//#pragma instancing_options procedural:vertInstancingSetup

			#pragma vertex vertEditorPass
			#pragma fragment fragSceneHighlightPass

			#include "UnityStandardParticleEditor.cginc"
			ENDCG
		}

		Pass
		{
			Name "ScenePickingPass"
			Tags{ "LightMode" = "Picking" }

			BlendOp Add
			Blend One Zero
			ZWrite On
			Cull Off

			CGPROGRAM
			#pragma target 4.5
			#pragma exclude_renderers gles gles3 glcore d3d11_9x wiiu n3ds switch

			#pragma multi_compile_instancing
			//#pragma instancing_options procedural:vertInstancingSetup

			#pragma vertex vertEditorPass
			#pragma fragment fragScenePickingPass

			#include "UnityStandardParticleEditor.cginc"
			ENDCG
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD BASE
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Stencil
			{
				Ref [_StencilRef]
				ReadMask [_StencilReadMask]
				WriteMask [_StencilWriteMask]
				Comp [_StencilComp]
				Pass [_StencilPass]
				Fail [_StencilFail]
				ZFail [_StencilZFail]
			}

			Tags { "LightMode" = "ForwardBase" } 
			Name "ForwardBase" 
			Cull [_RenderFace]
			Blend [_BlendSrc] [_BlendDst]
			ZWrite [_ZWrite]
			ZTest [_ZTest]
			ColorMask RGB

			HLSLPROGRAM
			#pragma target 4.5
			#pragma shader_feature_local __ _MK_SOFT_FADE
			#pragma shader_feature_local __ _MK_CAMERA_FADE
			#pragma shader_feature_local __ _MK_FLIPBOOK
			#pragma shader_feature_local __ _MK_COLOR_BLEND_ADDITIVE _MK_COLOR_BLEND_SUBTRACTIVE _MK_COLOR_BLEND_OVERLAY _MK_COLOR_BLEND_COLOR _MK_COLOR_BLEND_DIFFERENCE
			#pragma shader_feature_local __ _MK_LIGHT_CEL _MK_LIGHT_BANDED _MK_LIGHT_RAMP
			#pragma shader_feature_local __ _MK_THRESHOLD_MAP
			#pragma shader_feature_local __ _MK_ARTISTIC_DRAWN _MK_ARTISTIC_HATCHING _MK_ARTISTIC_SKETCH
			#pragma shader_feature_local __ _MK_ARTISTIC_PROJECTION_SCREEN_SPACE
			#pragma shader_feature_local __ _MK_ARTISTIC_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_NORMAL_MAP
			#pragma shader_feature_local __ _MK_SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local __ _MK_ALPHA_CLIPPING
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature __ _MK_EMISSION
			#pragma shader_feature __ _MK_EMISSION_MAP
			#pragma shader_feature_local __ _MK_ENVIRONMENT_REFLECTIONS_AMBIENT
			#pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_BLEND_PREMULTIPLY _MK_BLEND_ADDITIVE _MK_BLEND_MULTIPLY
			#pragma shader_feature_local __ _MK_SPECULAR_ISOTROPIC
			#pragma shader_feature_local __ _MK_RIM_DEFAULT _MK_RIM_SPLIT
			#pragma shader_feature_local __ _MK_IRIDESCENCE_DEFAULT
			#pragma shader_feature_local __ _MK_COLOR_GRADING_ALBEDO _MK_COLOR_GRADING_FINAL_OUTPUT
			#pragma shader_feature_local __ _MK_GOOCH_RAMP
			#pragma shader_feature_local __ _MK_WRAPPED_DIFFUSE
			#pragma shader_feature_local __ _MK_REFRACTION_DISTORTION_MAP
			#pragma shader_feature_local __ _MK_INDEX_OF_REFRACTION

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex ForwardVert
			#pragma fragment ForwardFrag

			#pragma exclude_renderers gles gles3 glcore d3d11_9x wiiu n3ds switch
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase
            #pragma multi_compile __ VERTEXLIGHT_ON

			#pragma multi_compile_instancing
			//#pragma instancing_options procedural:vertInstancingSetup

			#define MK_LEGACY_RP
			#define MK_REFRACTION
			#define MK_PARTICLES
			#define MK_SIMPLE

			#include "../../Lib/Forward/BaseSetup.hlsl"
			
			ENDHLSL
		}
		
		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD ADD
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Stencil
			{
				Ref [_StencilRef]
				ReadMask [_StencilReadMask]
				WriteMask [_StencilWriteMask]
				Comp [_StencilComp]
				Pass [_StencilPass]
				Fail [_StencilFail]
				ZFail [_StencilZFail]
			}
			
			Tags { "LightMode" = "ForwardAdd" } 
			Name "ForwardAdd"
			Cull [_RenderFace]
			Blend [_BlendSrc] One
			ZWrite [_ZWrite]
			ZTest [_ZTest]
			ColorMask RGB

			HLSLPROGRAM
			#pragma target 4.5
			#pragma shader_feature_local __ _MK_SOFT_FADE
			#pragma shader_feature_local __ _MK_CAMERA_FADE
			#pragma shader_feature_local __ _MK_FLIPBOOK
			#pragma shader_feature_local __ _MK_COLOR_BLEND_ADDITIVE _MK_COLOR_BLEND_SUBTRACTIVE _MK_COLOR_BLEND_OVERLAY _MK_COLOR_BLEND_COLOR _MK_COLOR_BLEND_DIFFERENCE
			#pragma shader_feature_local __ _MK_LIGHT_CEL _MK_LIGHT_BANDED _MK_LIGHT_RAMP
			#pragma shader_feature_local __ _MK_THRESHOLD_MAP
			#pragma shader_feature_local __ _MK_ARTISTIC_DRAWN _MK_ARTISTIC_HATCHING _MK_ARTISTIC_SKETCH
			#pragma shader_feature_local __ _MK_ARTISTIC_PROJECTION_SCREEN_SPACE
			#pragma shader_feature_local __ _MK_ARTISTIC_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_NORMAL_MAP
			#pragma shader_feature_local __ _MK_SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local __ _MK_ALPHA_CLIPPING
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_ENVIRONMENT_REFLECTIONS_AMBIENT
			#pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_BLEND_PREMULTIPLY _MK_BLEND_ADDITIVE _MK_BLEND_MULTIPLY
			#pragma shader_feature_local __ _MK_SPECULAR_ISOTROPIC
			#pragma shader_feature_local __ _MK_RIM_SPLIT
			#pragma shader_feature_local __ _MK_COLOR_GRADING_ALBEDO _MK_COLOR_GRADING_FINAL_OUTPUT
			#pragma shader_feature_local __ _MK_GOOCH_RAMP
			#pragma shader_feature_local __ _MK_WRAPPED_DIFFUSE

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex ForwardVert
			#pragma fragment ForwardFrag

			#pragma exclude_renderers gles gles3 glcore d3d11_9x wiiu n3ds switch
			#pragma multi_compile_fog
			#pragma multi_compile_fwdadd_fullshadows

			#define MK_LEGACY_RP
			#define MK_PARTICLES
			#define MK_SIMPLE

			#include "../../Lib/Forward/AddSetup.hlsl"
			
			ENDHLSL
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// DEFERRED
		/////////////////////////////////////////////////////////////////////////////////////////////

		/////////////////////////////////////////////////////////////////////////////////////////////
		// SHADOWCASTER
		/////////////////////////////////////////////////////////////////////////////////////////////

		/////////////////////////////////////////////////////////////////////////////////////////////
		// META
		/////////////////////////////////////////////////////////////////////////////////////////////
    }
	
	/////////////////////////////////////////////////////////////////////////////////////////////
	// SM 3.5
	/////////////////////////////////////////////////////////////////////////////////////////////
	SubShader
	{
		Tags {"RenderType"="Transparent" "PerformanceChecks"="False" "IgnoreProjector" = "True" "PreviewType" = "Plane"}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// REFRACTION
		/////////////////////////////////////////////////////////////////////////////////////////////
		GrabPass { Tags { "LightMode" = "Always" } "_MKToonRefraction" Name "MKSharedRefraction" }

		/////////////////////////////////////////////////////////////////////////////////////////////
		// Editor Only
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Name "SceneSelectionPass"
			Tags { "LightMode" = "SceneSelectionPass" }

			BlendOp Add
			Blend One Zero
			ZWrite On
			Cull Off

			CGPROGRAM
			#pragma target 3.5
			#pragma exclude_renderers gles d3d11_9x ps4 ps5 xboxone

			#pragma multi_compile_instancing
			//#pragma instancing_options procedural:vertInstancingSetup

			#pragma vertex vertEditorPass
			#pragma fragment fragSceneHighlightPass

			#include "UnityStandardParticleEditor.cginc"
			ENDCG
		}

		Pass
		{
			Name "ScenePickingPass"
			Tags{ "LightMode" = "Picking" }

			BlendOp Add
			Blend One Zero
			ZWrite On
			Cull Off

			CGPROGRAM
			#pragma target 3.5
			#pragma exclude_renderers gles d3d11_9x ps4 ps5 xboxone

			#pragma multi_compile_instancing
			//#pragma instancing_options procedural:vertInstancingSetup

			#pragma vertex vertEditorPass
			#pragma fragment fragScenePickingPass

			#include "UnityStandardParticleEditor.cginc"
			ENDCG
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD BASE
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Stencil
			{
				Ref [_StencilRef]
				ReadMask [_StencilReadMask]
				WriteMask [_StencilWriteMask]
				Comp [_StencilComp]
				Pass [_StencilPass]
				Fail [_StencilFail]
				ZFail [_StencilZFail]
			}

			Tags { "LightMode" = "ForwardBase" } 
			Name "ForwardBase" 
			Cull [_RenderFace]
			Blend [_BlendSrc] [_BlendDst]
			ZWrite [_ZWrite]
			ZTest [_ZTest]
			ColorMask RGB

			HLSLPROGRAM
			#pragma target 3.5
			#pragma shader_feature_local __ _MK_SOFT_FADE
			#pragma shader_feature_local __ _MK_CAMERA_FADE
			#pragma shader_feature_local __ _MK_FLIPBOOK
			#pragma shader_feature_local __ _MK_COLOR_BLEND_ADDITIVE _MK_COLOR_BLEND_SUBTRACTIVE _MK_COLOR_BLEND_OVERLAY _MK_COLOR_BLEND_COLOR _MK_COLOR_BLEND_DIFFERENCE
			#pragma shader_feature_local __ _MK_LIGHT_CEL _MK_LIGHT_BANDED _MK_LIGHT_RAMP
			#pragma shader_feature_local __ _MK_THRESHOLD_MAP
			#pragma shader_feature_local __ _MK_ARTISTIC_DRAWN _MK_ARTISTIC_HATCHING _MK_ARTISTIC_SKETCH
			#pragma shader_feature_local __ _MK_ARTISTIC_PROJECTION_SCREEN_SPACE
			#pragma shader_feature_local __ _MK_ARTISTIC_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_NORMAL_MAP
			#pragma shader_feature_local __ _MK_SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local __ _MK_ALPHA_CLIPPING
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature __ _MK_EMISSION
			#pragma shader_feature __ _MK_EMISSION_MAP
			#pragma shader_feature_local __ _MK_ENVIRONMENT_REFLECTIONS_AMBIENT
			#pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_BLEND_PREMULTIPLY _MK_BLEND_ADDITIVE _MK_BLEND_MULTIPLY
			#pragma shader_feature_local __ _MK_SPECULAR_ISOTROPIC
			#pragma shader_feature_local __ _MK_RIM_DEFAULT _MK_RIM_SPLIT
			#pragma shader_feature_local __ _MK_IRIDESCENCE_DEFAULT
			#pragma shader_feature_local __ _MK_COLOR_GRADING_ALBEDO _MK_COLOR_GRADING_FINAL_OUTPUT
			#pragma shader_feature_local __ _MK_GOOCH_RAMP
			#pragma shader_feature_local __ _MK_WRAPPED_DIFFUSE
			#pragma shader_feature_local __ _MK_REFRACTION_DISTORTION_MAP
			#pragma shader_feature_local __ _MK_INDEX_OF_REFRACTION

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex ForwardVert
			#pragma fragment ForwardFrag

			#pragma exclude_renderers gles d3d11_9x ps4 ps5 xboxone
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase
            #pragma multi_compile __ VERTEXLIGHT_ON

			#pragma multi_compile_instancing
			//#pragma instancing_options procedural:vertInstancingSetup

			#define MK_LEGACY_RP
			#define MK_REFRACTION
			#define MK_PARTICLES
			#define MK_SIMPLE

			#include "../../Lib/Forward/BaseSetup.hlsl"
			
			ENDHLSL
		}
		
		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD ADD
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Stencil
			{
				Ref [_StencilRef]
				ReadMask [_StencilReadMask]
				WriteMask [_StencilWriteMask]
				Comp [_StencilComp]
				Pass [_StencilPass]
				Fail [_StencilFail]
				ZFail [_StencilZFail]
			}
			
			Tags { "LightMode" = "ForwardAdd" } 
			Name "ForwardAdd"
			Cull [_RenderFace]
			Blend [_BlendSrc] One
			ZWrite [_ZWrite]
			ZTest [_ZTest]
			ColorMask RGB

			HLSLPROGRAM
			#pragma target 3.5
			#pragma shader_feature_local __ _MK_SOFT_FADE
			#pragma shader_feature_local __ _MK_CAMERA_FADE
			#pragma shader_feature_local __ _MK_FLIPBOOK
			#pragma shader_feature_local __ _MK_COLOR_BLEND_ADDITIVE _MK_COLOR_BLEND_SUBTRACTIVE _MK_COLOR_BLEND_OVERLAY _MK_COLOR_BLEND_COLOR _MK_COLOR_BLEND_DIFFERENCE
			#pragma shader_feature_local __ _MK_LIGHT_CEL _MK_LIGHT_BANDED _MK_LIGHT_RAMP
			#pragma shader_feature_local __ _MK_THRESHOLD_MAP
			#pragma shader_feature_local __ _MK_ARTISTIC_DRAWN _MK_ARTISTIC_HATCHING _MK_ARTISTIC_SKETCH
			#pragma shader_feature_local __ _MK_ARTISTIC_PROJECTION_SCREEN_SPACE
			#pragma shader_feature_local __ _MK_ARTISTIC_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_NORMAL_MAP
			#pragma shader_feature_local __ _MK_SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local __ _MK_ALPHA_CLIPPING
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_ENVIRONMENT_REFLECTIONS_AMBIENT
			#pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_BLEND_PREMULTIPLY _MK_BLEND_ADDITIVE _MK_BLEND_MULTIPLY
			#pragma shader_feature_local __ _MK_SPECULAR_ISOTROPIC
			#pragma shader_feature_local __ _MK_RIM_SPLIT
			#pragma shader_feature_local __ _MK_COLOR_GRADING_ALBEDO _MK_COLOR_GRADING_FINAL_OUTPUT
			#pragma shader_feature_local __ _MK_GOOCH_RAMP
			#pragma shader_feature_local __ _MK_WRAPPED_DIFFUSE

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex ForwardVert
			#pragma fragment ForwardFrag

			#pragma exclude_renderers gles d3d11_9x ps4 ps5 xboxone
			#pragma multi_compile_fog
			#pragma multi_compile_fwdadd_fullshadows

			#define MK_LEGACY_RP
			#define MK_PARTICLES
			#define MK_SIMPLE

			#include "../../Lib/Forward/AddSetup.hlsl"
			
			ENDHLSL
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// DEFERRED
		/////////////////////////////////////////////////////////////////////////////////////////////

		/////////////////////////////////////////////////////////////////////////////////////////////
		// SHADOWCASTER
		/////////////////////////////////////////////////////////////////////////////////////////////

		/////////////////////////////////////////////////////////////////////////////////////////////
		// META
		/////////////////////////////////////////////////////////////////////////////////////////////
    }

	/////////////////////////////////////////////////////////////////////////////////////////////
	// SM 2.5
	/////////////////////////////////////////////////////////////////////////////////////////////
	SubShader
	{
		Tags {"RenderType"="Transparent" "PerformanceChecks"="False" "IgnoreProjector" = "True" "PreviewType" = "Plane"}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// REFRACTION
		/////////////////////////////////////////////////////////////////////////////////////////////
		GrabPass { Tags { "LightMode" = "Always" } "_MKToonRefraction" Name "MKSharedRefraction" }

		/////////////////////////////////////////////////////////////////////////////////////////////
		// Editor Only
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Name "SceneSelectionPass"
			Tags { "LightMode" = "SceneSelectionPass" }

			BlendOp Add
			Blend One Zero
			ZWrite On
			Cull Off

			CGPROGRAM
			#pragma target 2.5
			#pragma exclude_renderers gles3 d3d11 ps4 ps5 xboxone wiiu n3ds switch

			#pragma multi_compile_instancing
			//#pragma instancing_options procedural:vertInstancingSetup

			#pragma vertex vertEditorPass
			#pragma fragment fragSceneHighlightPass

			#include "UnityStandardParticleEditor.cginc"
			ENDCG
		}

		Pass
		{
			Name "ScenePickingPass"
			Tags{ "LightMode" = "Picking" }

			BlendOp Add
			Blend One Zero
			ZWrite On
			Cull Off

			CGPROGRAM
			#pragma target 2.5
			#pragma exclude_renderers gles3 d3d11 ps4 ps5 xboxone wiiu n3ds switch

			#pragma multi_compile_instancing
			//#pragma instancing_options procedural:vertInstancingSetup

			#pragma vertex vertEditorPass
			#pragma fragment fragScenePickingPass

			#include "UnityStandardParticleEditor.cginc"
			ENDCG
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD BASE
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Stencil
			{
				Ref [_StencilRef]
				ReadMask [_StencilReadMask]
				WriteMask [_StencilWriteMask]
				Comp [_StencilComp]
				Pass [_StencilPass]
				Fail [_StencilFail]
				ZFail [_StencilZFail]
			}

			Tags { "LightMode" = "ForwardBase" } 
			Name "ForwardBase" 
			Cull [_RenderFace]
			Blend [_BlendSrc] [_BlendDst]
			ZWrite [_ZWrite]
			ZTest [_ZTest]
			ColorMask RGB

			HLSLPROGRAM
			#pragma target 2.5
			#pragma shader_feature_local __ _MK_SOFT_FADE
			#pragma shader_feature_local __ _MK_CAMERA_FADE
			#pragma shader_feature_local __ _MK_COLOR_BLEND_ADDITIVE _MK_COLOR_BLEND_SUBTRACTIVE _MK_COLOR_BLEND_OVERLAY _MK_COLOR_BLEND_COLOR _MK_COLOR_BLEND_DIFFERENCE
			#pragma shader_feature_local __ _MK_LIGHT_CEL _MK_LIGHT_BANDED _MK_LIGHT_RAMP
			#pragma shader_feature_local __ _MK_THRESHOLD_MAP
			#pragma shader_feature_local __ _MK_ARTISTIC_DRAWN _MK_ARTISTIC_HATCHING _MK_ARTISTIC_SKETCH
			#pragma shader_feature_local __ _MK_ARTISTIC_PROJECTION_SCREEN_SPACE
			#pragma shader_feature_local __ _MK_ARTISTIC_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local __ _MK_ALPHA_CLIPPING
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature __ _MK_EMISSION
			#pragma shader_feature __ _MK_EMISSION_MAP
			#pragma shader_feature_local __ _MK_ENVIRONMENT_REFLECTIONS_AMBIENT
			#pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_BLEND_PREMULTIPLY _MK_BLEND_ADDITIVE _MK_BLEND_MULTIPLY
			#pragma shader_feature_local __ _MK_SPECULAR_ISOTROPIC
			#pragma shader_feature_local __ _MK_RIM_DEFAULT _MK_RIM_SPLIT
			#pragma shader_feature_local __ _MK_IRIDESCENCE_DEFAULT
			#pragma shader_feature_local __ _MK_COLOR_GRADING_ALBEDO _MK_COLOR_GRADING_FINAL_OUTPUT
			#pragma shader_feature_local __ _MK_GOOCH_RAMP
			#pragma shader_feature_local __ _MK_WRAPPED_DIFFUSE
			#pragma shader_feature_local __ _MK_REFRACTION_DISTORTION_MAP
			#pragma shader_feature_local __ _MK_INDEX_OF_REFRACTION

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex ForwardVert
			#pragma fragment ForwardFrag

			#pragma exclude_renderers gles3 d3d11 ps4 ps5 xboxone wiiu n3ds switch
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase
            #pragma multi_compile __ VERTEXLIGHT_ON

			#pragma multi_compile_instancing
			//#pragma instancing_options procedural:vertInstancingSetup

			#define MK_LEGACY_RP
			#define MK_REFRACTION
			#define MK_PARTICLES
			#define MK_SIMPLE

			#include "../../Lib/Forward/BaseSetup.hlsl"
			
			ENDHLSL
		}
		
		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD ADD
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Stencil
			{
				Ref [_StencilRef]
				ReadMask [_StencilReadMask]
				WriteMask [_StencilWriteMask]
				Comp [_StencilComp]
				Pass [_StencilPass]
				Fail [_StencilFail]
				ZFail [_StencilZFail]
			}
			
			Tags { "LightMode" = "ForwardAdd" } 
			Name "ForwardAdd"
			Cull [_RenderFace]
			Blend [_BlendSrc] One
			ZWrite [_ZWrite]
			ZTest [_ZTest]
			ColorMask RGB

			HLSLPROGRAM
			#pragma target 2.5
			#pragma shader_feature_local __ _MK_SOFT_FADE
			#pragma shader_feature_local __ _MK_CAMERA_FADE
			#pragma shader_feature_local __ _MK_COLOR_BLEND_ADDITIVE _MK_COLOR_BLEND_SUBTRACTIVE _MK_COLOR_BLEND_OVERLAY _MK_COLOR_BLEND_COLOR _MK_COLOR_BLEND_DIFFERENCE
			#pragma shader_feature_local __ _MK_LIGHT_CEL _MK_LIGHT_BANDED _MK_LIGHT_RAMP
			#pragma shader_feature_local __ _MK_THRESHOLD_MAP
			#pragma shader_feature_local __ _MK_ARTISTIC_DRAWN _MK_ARTISTIC_HATCHING _MK_ARTISTIC_SKETCH
			#pragma shader_feature_local __ _MK_ARTISTIC_PROJECTION_SCREEN_SPACE
			#pragma shader_feature_local __ _MK_ARTISTIC_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local __ _MK_ALPHA_CLIPPING
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_STUTTER
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_SINE _MK_VERTEX_ANIMATION_PULSE _MK_VERTEX_ANIMATION_NOISE
			#pragma shader_feature_local __ _MK_VERTEX_ANIMATION_MAP
			#pragma shader_feature_local __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_BORDER_COLOR _MK_DISSOLVE_BORDER_RAMP
			#pragma shader_feature_local __ _MK_ENVIRONMENT_REFLECTIONS_AMBIENT
			#pragma shader_feature_local __ _MK_ALBEDO_MAP
            #pragma shader_feature_local __ _MK_BLEND_PREMULTIPLY _MK_BLEND_ADDITIVE _MK_BLEND_MULTIPLY
			#pragma shader_feature_local __ _MK_SPECULAR_ISOTROPIC
			#pragma shader_feature_local __ _MK_RIM_SPLIT
			#pragma shader_feature_local __ _MK_COLOR_GRADING_ALBEDO _MK_COLOR_GRADING_FINAL_OUTPUT
			#pragma shader_feature_local __ _MK_GOOCH_RAMP
			#pragma shader_feature_local __ _MK_WRAPPED_DIFFUSE

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex ForwardVert
			#pragma fragment ForwardFrag

			#pragma exclude_renderers gles3 d3d11 ps4 ps5 xboxone wiiu n3ds switch
			#pragma multi_compile_fog
			#pragma multi_compile_fwdadd_fullshadows

			#define MK_LEGACY_RP
			#define MK_PARTICLES
			#define MK_SIMPLE

			#include "../../Lib/Forward/AddSetup.hlsl"
			
			ENDHLSL
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// DEFERRED
		/////////////////////////////////////////////////////////////////////////////////////////////

		/////////////////////////////////////////////////////////////////////////////////////////////
		// SHADOWCASTER
		/////////////////////////////////////////////////////////////////////////////////////////////

		/////////////////////////////////////////////////////////////////////////////////////////////
		// META
		/////////////////////////////////////////////////////////////////////////////////////////////
    }
	
	FallBack Off
	CustomEditor "MK.Toon.Editor.Legacy.ParticlesSimpleEditor"
}
