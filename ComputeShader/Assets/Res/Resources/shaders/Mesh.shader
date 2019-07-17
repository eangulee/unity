// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Mesh"
{
	Properties
	{
		_Color("Color",color)=(0.5,0.5,0.5,1)
		_MainTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		CGINCLUDE
		#pragma multi_compile_fog
		#pragma target 5.0
		#include "UnityCG.cginc"
		#include "AutoLight.cginc"
		#include "ComputeBuffer.cginc"

		uniform StructuredBuffer<Particle> _Particles;
		uniform float _IdOffset;
		uniform fixed4 _Color;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float4 _LightColor0;
		inline int GetID(float2 uv)
		{
			return (int)(uv.x + 0.5 + _IdOffset);
		}
		ENDCG
		Pass
		{
			Tags{"LightMode" = "Deferred"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#pragma multi_compile ___ UNITY_HDR_ON

			struct G_Buffer
			{
				fixed4 diffuse : SV_Target0;
				float4 specSmoothness : SV_Target1;
				float4 normal : SV_Target2;
				fixed4 emission : SV_Target3;
			};
				struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
				float2 id : TEXCOORD1;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
				float4 color : TEXCOORD1;
			};
			v2f vert(a2v v)
			{
				Particle p = _Particles[GetID(v.id)];
				v.vertex.xyz *= p.scale;
				v.vertex.xyz += p.position;

				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = p.uv;
				o.color = p.color;
				o.normal = UnityObjectToWorldNormal(v.normal);
				return o;
			}
			G_Buffer frag(v2f i)
			{
				i.normal = normalize(i.normal);
				fixed4 col = tex2D(_MainTex, i.uv)*i.color;
				clip(col.a - 0.2);

				G_Buffer g;
				g.diffuse = _Color;
				g.specSmoothness = 0;
				g.normal = half4(i.normal * 0.5 + 0.5, 1);
				g.emission = col;
			#ifndef UNITY_HDR_ON
				g.emission.rgb = exp2(-g.emission.rgb);
			#endif
				return g;
			}
			ENDCG
		}

		Pass
		{
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			ZTest LEqual
			Offset 1, 1

			CGPROGRAM
			#pragma vertex vert_
			#pragma fragment frag_
			#pragma multi_compile_shadowcaster

			struct a2v_ {
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
			float2 id : TEXCOORD1;
			};
			struct v2f_ {
				V2F_SHADOW_CASTER;
				float2 uv : TEXCOORD1;
			};
			v2f_ vert_(a2v_ v) {
				Particle p = _Particles[GetID(v.id)];
				v.vertex.xyz *= p.scale;
				v.vertex.xyz += p.position;

				v2f_ o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = p.uv;
				TRANSFER_SHADOW_CASTER(o)
				return o;
			}
			float4 frag_(v2f_ i) : COLOR{
				float4 col = tex2D(_MainTex,i.uv);
				clip(col.a - 0.2);
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
	}
FallBack "Diffuse"
}