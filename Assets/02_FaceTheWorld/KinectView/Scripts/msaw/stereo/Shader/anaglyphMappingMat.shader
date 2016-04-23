Shader "stereo/anaglyphMappingMat"
{
	Properties
	{
		//_MainTex ("Texture", 2D) = "white" {}
		_Color ("Main Color, Alpha", Color) = (1,1,1,1)
		_FrontTex ("Left (RGB)", RECT) = "white" {} 
        _BackTex ("Right (RGB)", RECT) = "white" {}
        _EdgeBlend ("Culling Mask", 2D) = "black" {}
        _Cutoff ("Alpha cutoff", Range (0,1)) = 0.1
		
	}
	Category
	{
	 
		// No culling or depth
		// Cull Off 
		ZWrite Off 
		ZTest Always 
		Lighting On 
		Blend SrcAlpha OneMinusSrcAlpha
		//Blend Zero OneMinusDstAlpha
		 //Alphatest LEqual 0.5
		Tags{Queue = Transparent}
		SubShader
		{
		
			Pass 
			{
				ColorMask RGB
				Cull Off
				Material 
				{
					Emission [_Color]
				}  
				
				SetTexture [_LeftTex] 
				{ 
					Combine texture * primary, texture + primary 
				} 
				SetTexture [_EdgeBlend] 
				{ 
				//o.Albedo = texture.rgb;
  				//o.Alpha = (1 - texture.a);
  				//texture.a = (1 - texture.a);
  				
					Combine Previous ,  texture * previous
				}    
				  
				
			}
		           
			Pass 
			{
				ColorMask RGB
				Cull Off 
				Material 
				{
					Emission [_Color] 
				} 
				SetTexture [_RightTex] 
				{
					Combine texture * primary, texture + primary
				} 
				SetTexture [_EdgeBlend] 
				{ 
					Combine Previous ,    texture * previous
				}    
			} 
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
