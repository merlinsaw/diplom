Shader "Custom/ClipShaderRotational"
{
   Properties
   {
      _MainTex ("Texture", 2D) = "white" {}
      _x ("_x",Float) = 0.0
      _y ("_y",Float) = 0.0
      _z ("_z",Float) = 0.0
      _direction ("_direction",Float) = 0.0
      _clip ("_clip",Float) = 1
      _ZeroParalax ("_ZeroParalax", Vector) = (0,0,0,0)
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      Cull back
      CGPROGRAM
      #pragma surface surf Lambert
      struct Input {
          float2 uv_MainTex;
          float3 worldPos;
      };
     
      sampler2D _MainTex;
      float _x,_y,_z,_direction,_clip;
 
      void surf (Input IN, inout SurfaceOutput o)
      {
          if(_clip == 1)
          {
             clip (_x * IN.worldPos.x +
                _y *IN.worldPos.y +
                _z *IN.worldPos.z +
                _direction > 0 ? -1 :1);
         }
 
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
      }
      ENDCG
    }
    Fallback "Diffuse"
  }
 