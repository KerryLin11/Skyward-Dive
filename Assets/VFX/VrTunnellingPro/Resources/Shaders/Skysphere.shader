﻿Shader "Hidden/VrTunnellingPro/Skysphere" {
	Properties {
		_Skybox ("Texture", CUBE) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
	}
	CGINCLUDE
	#pragma vertex vert
	#include "UnityCG.cginc"

	struct appdata {
		float4 vertex : POSITION;
		float3 uv : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f {
		float3 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	samplerCUBE _Skybox;
	fixed3 _Color;
	
	v2f vert (appdata v) {
		v2f o;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_OUTPUT(v2f, o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		o.vertex = UnityObjectToClipPos(float4(v.vertex.xyz, 0));
		o.vertex.z = o.vertex.w;
		o.uv = v.vertex.xyz;
		return o;
	}
	
	fixed3 frag (v2f i) : SV_Target {
		return texCUBE(_Skybox, i.uv).rgb * _Color;
	}
	fixed4 frag4 (v2f i) : SV_Target {
		return fixed4(frag(i), 1);
	}
	ENDCG

	// Regular subshader
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Background" }
		LOD 100
		Cull Off
		ZWrite Off
		Colormask RGB

		Pass {
			CGPROGRAM
			#pragma fragment frag
			#pragma multi_compile_instancing
			#pragma exclude_renderers metal
			ENDCG
		}
	}
	// Metal subshader (RGBA)
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Background" }
		LOD 100
		Cull Off
		ZWrite Off

		Pass {
			CGPROGRAM
			#pragma fragment frag4
			#pragma multi_compile_instancing
			#pragma only_renderers metal
			ENDCG
		}
	}
}
