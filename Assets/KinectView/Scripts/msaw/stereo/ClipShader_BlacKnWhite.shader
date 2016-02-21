Shader "Custom/ClipShaderBlacKnWhite"
{
   Properties
   {
      _MainTex ("Texture", 2D) = "white" {}
      _Brightness("Brightness Value", Range(-0.5, 0.5)) = 0
      _Contrast("Contrast Value", Range(-0.5, 0.5)) = 0
      _a ("_a",Float) = 0.0
      _b ("_b",Float) = 0.0
      _c ("_c",Float) = 0.0
      _d ("_d",Float) = 0.0
      _clip ("_clip",Float) = 1
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      Cull Off Lighting Off
      CGPROGRAM
      #pragma surface surf Lambert
      struct Input {
          float2 uv_MainTex;
          float3 worldPos;
      };
     uniform float _testVar;
     uniform float _Brightness;
     uniform float _Contrast;
     
      sampler2D _MainTex;
      float _a,_b,_c,_d,_clip;
 
      void surf (Input IN, inout SurfaceOutput o)
      {
          if(_clip == 1)
          {
             clip (_a *IN.worldPos.x +
                _b *IN.worldPos.y +
                _c *(IN.worldPos.z-396.0F) +
                _d > 0 ? -1 :1);
         }
 
 				// Applying Contrast
                  float factor = (1.0156862 * (_Contrast + 1)) / (1 * (1.0156862 - _Contrast)) ;
                  float red = tex2D (_MainTex, IN.uv_MainTex).r ;
                  float green = tex2D (_MainTex, IN.uv_MainTex).g ;
                  float blue = tex2D (_MainTex, IN.uv_MainTex).b ;
                   red = (factor * (red - 0.5) + 0.5) ;
                   green = (factor * (green - 0.5) + 0.5) ;
                   blue = (factor * (blue - 0.5) + 0.5) ;
                   // end Contrast
                   
                   // Applying Brightness
                   red = red + _Brightness ;
                   green = green + _Brightness ;
                   blue = blue + _Brightness ;
                   // end Brightness
                   
                    
                   // Applying B & W
//                   float average = (red+green+blue) / 3 ;
                   float average = 0.30 * red + 0.59 * green + 0.11 * blue ;
                   red = green = blue = average ;
                   // end B & W
                   
                   // BW 
                   o.Albedo = float3 (red, green, blue) ; 
          //Clip o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
          //o.Albedo = tex2D (float3 (red, green, blue), IN.uv_MainTex).rgb;
      }
      ENDCG
    }
    Fallback "Diffuse"
  }
 