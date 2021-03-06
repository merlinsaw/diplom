﻿Shader "stereo/anaglyphMatAndMasking"
{
	Properties
	{
		//_MainTex ("Texture", 2D) = "white" {}
		_Color ("Main Color, Alpha", Color) = (1,1,1,1)
		_LeftTex ("Left (RGB)", RECT) = "white" {} 
        _RightTex ("Right (RGB)", RECT) = "white" {} 
        _LeftMask ("Mask (A)", RECT) = "white" {}
        _RightMask ("Mask (A)", RECT) = "white" {}
		
	}
	Category
	{
		// No culling or depth
		//Cull Off 
		ZWrite off 
		ZTest Always 
		//Lighting On 
		Tags{Queue = Transparent}
		SubShader
		{
		
			Pass 
			{
				Lighting On
				ColorMask R 
				Cull off
				Material 
				{
					Emission [_Color]
				} 
		              
				SetTexture [_LeftTex] 
				{ 
					Combine texture * primary, texture + primary 
				}
				SetTexture [_LeftMask]
				{
					Combine texture * previous, texture
				}
				SetTexture [_RightMask]
				{
					Combine texture * previous, texture
				}
			}
		           
			Pass 
			{
				Lighting On
				ColorMask GB
				Cull Off 
				Material 
				{
					Emission [_Color] 
				} 
				SetTexture [_RightTex] 
				{
					Combine texture * primary, texture + primary
				} 
				SetTexture [_RightMask]
				{
					Combine texture * previous, texture
				}
				SetTexture [_LeftMask]
				{
					Combine texture * previous, texture
				}
			} 
//			Pass 
//			{
//				Lighting Off 
//				ColorMask RGBA
//				Cull Off 
//				Material 
//				{
//					Emission [_Color] 
//				} 
//				SetTexture [_LeftMask] 
//				{
//					//Combine texture * primary, texture - primary
//					combine texture lerp (texture) primary
//				} 
//			} 
//			Pass 
//			{
//				Lighting Off 
//				ColorMask RGBA
//				Cull Off 
//				Material 
//				{
//					Emission [_Color] 
//				} 
//				SetTexture [_RightMask] 
//				{
//					Combine texture * primary, texture - primary
//				} 
//			} 
		}


//		Pass
//		{
//			CGPROGRAM
//			#pragma vertex vert
//			#pragma fragment frag
//			
//			#include "UnityCG.cginc"
//
//			struct appdata
//			{
//				float4 vertex : POSITION;
//				float2 uv : TEXCOORD0;
//			};
//
//			struct v2f
//			{
//				float2 uv : TEXCOORD0;
//				float4 vertex : SV_POSITION;
//			};
//
//			v2f vert (appdata v)
//			{
//				v2f o;
//				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
//				o.uv = v.uv;
//				return o;
//			}
//			
//			sampler2D _MainTex;
//
//			fixed4 frag (v2f i) : SV_Target
//			{
//				fixed4 col = tex2D(_MainTex, i.uv);
//				// just invert the colors
//				col = 1 - col;
//				return col;
//			}
//			ENDCG
//		}
	}
}
