Shader "Custom/MyShader"
{
	Properties
	{
	   _Color("Main Color", Color) = (1,1,1,1)
	   _MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	   _BlendTex("Blend (RGB)", 2D) = "white"
	   _BlendAlpha("Blend Alpha", float) = 0
	}
		SubShader
	   {
		  Tags { "Queue" = "Geometry-9" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		  Lighting Off
		  LOD 200
		  Blend SrcAlpha OneMinusSrcAlpha

		  CGPROGRAM
		  #pragma surface surf Lambert

		  fixed4 _Color;
		  sampler2D _MainTex;
		  sampler2D _BlendTex;
		  float _BlendAlpha;

		  struct Input {
			float2 uv_MainTex;
		  };

		  void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = ((1 - _BlendAlpha) * tex2D(_MainTex, IN.uv_MainTex) + _BlendAlpha * tex2D(_BlendTex, IN.uv_MainTex)) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		  }
		  ENDCG
	   }

		   Fallback "Transparent/VertexLit"
}