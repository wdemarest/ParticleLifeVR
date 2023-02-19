Shader "Custom/Fog2"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard finalcolor:mycolor vertex:myvert
		#pragma multi_compile_fog

        sampler2D _MainTex;
		uniform half4 unity_FogStart; 
		uniform half4 unity_FogEnd;

        struct Input
        {
            float2 uv_MainTex;
			half fog;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

		void myvert(inout appdata_full v, out Input data)
		{
			UNITY_INITIALIZE_OUTPUT(Input, data);
			float horiz = length(UnityObjectToViewPos(v.vertex).xyz);
			float vertScalar = 60.0;
			float vert = clamp(1 - ((v.vertex.y - 60) / (120 - 60)), 0.0, 1.0) * vertScalar;
			float pos = max(horiz, vert);
			float diff = unity_FogEnd.x - unity_FogStart.x;
			float invDiff = 1.0f / diff;
			data.fog = clamp((unity_FogEnd.x - pos) * invDiff, 0.0, 1.0);
		}
		void mycolor(Input IN, SurfaceOutputStandard o, inout fixed4 color)
		{
#ifdef UNITY_PASS_FORWARDADD
			UNITY_APPLY_FOG_COLOR(IN.fog, color, float4(0, 0, 0, 0));
#else
			UNITY_APPLY_FOG_COLOR(IN.fog, color, unity_FogColor);
#endif
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
