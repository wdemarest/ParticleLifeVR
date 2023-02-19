
uniform float3 _colorAbove;
uniform float3 _colorTop;
uniform float3 _colorBottom;
uniform half _yTop;
uniform half _yBottom;
uniform half _viewMax;
uniform half _viewMin;

void DadFog3(inout appdata_full v, inout Input o)
{
	float4 vertPos = mul(unity_ObjectToWorld, v.vertex);
	float3 eyePos = _WorldSpaceCameraPos;
	float distFromEye = length(UnityObjectToViewPos(v.vertex).xyz);

	float ySpan = _yTop - _yBottom;

	// 0.0 = high and visible, 1.0 = low and fogged
	float vertHazeRaw = (_yTop - vertPos.y) / ySpan;
	float eyeHazeRaw = (_yTop - eyePos.y) / ySpan;

	float vertHaze = clamp(vertHazeRaw, 0.0, 1.0);
	float eyeHaze = clamp(eyeHazeRaw, 0.0, 1.0);

	float haze = (vertHaze + eyeHaze) * 0.5 * distFromEye;

	// meh. Nothing seems to give me the cragients that I want.
	// Maybe I should go look up the original fog formula, and alter it by altitude...

	// Fog of 1.0 means you can see it perfectly.
	o.fog = 1.0 - haze;
	o.alt = 1.0 - vertHaze;
}

void DadFinalColor3(Input IN, inout fixed4 color)
{
#ifdef UNITY_PASS_FORWARDADD
	UNITY_APPLY_FOG_COLOR(IN.fog, color, float4(0, 0, 0, 0));
#else
	//float3 target = IN.alt < 0 ? lerp(_colorTop.rgb, _colorAbove.rgb, -IN.alt) : lerp(_colorTop.rgb, _colorBottom.rgb, IN.alt);
	//color.rgb = lerp(target.rgb, color.rgb, IN.fog);	// Note that other things call saturate(IN.fog), but I find it does nothing?
	
	color.rgb.r = min(IN.alt, color.rgb.r);
	color.rgb.g = min(IN.alt, color.rgb.g);
	color.rgb.b = min(IN.alt, color.rgb.b);

	color.rgb = lerp(_colorBottom.rgb, color.rgb, IN.fog);	// Note that other things call saturate(IN.fog), but I find it does nothing?
	//color.rgb.r = IN.fog; // (color.rgb.r * 0.5) + (color.rgb.r * 0.5 * IN.fog);
	//color.rgb.g = IN.fog; //(color.rgb.g * 0.5) + (color.rgb.g * 0.5 * IN.fog);
	//color.rgb.b = IN.fog; //(color.rgb.b * 0.5) + (color.rgb.b * 0.5 * IN.fog);
#endif
}



void DadFog2( inout appdata_full v, inout Input o )
{
	float3 vertPos = mul(unity_ObjectToWorld, v.vertex);
	float3 eyePos = _WorldSpaceCameraPos;

	float3 usePos;
	usePos.x = eyePos.x;
	usePos.y = vertPos.y;
	usePos.z = eyePos.z;

	float distFromEye = length(UnityWorldToViewPos(vertPos).xyz);

	float ySpan = _yTop - _yBottom;

	// 0.0 = high and visible, 1.0 = low and fogged
	float vertHazeRaw = (_yTop - vertPos.y) / ySpan;
	//float eyeHazeRaw = (_yTop - eyePos.y) / ySpan;

	float vertHaze = clamp(vertHazeRaw, 0, 1);
	float viewDist = _viewMax - vertHaze * (_viewMax - _viewMin);
	float vertDist = distFromEye / viewDist;

	// Fog of 1.0 means you can see it perfectly.
	o.fog = 1 - vertDist;
	o.alt = vertHazeRaw;
}

void DadFinalColor2(Input IN, inout fixed4 color)
{
#ifdef UNITY_PASS_FORWARDADD
	UNITY_APPLY_FOG_COLOR(IN.fog, color, float4(0, 0, 0, 0));
#else
	//float3 target = lerp(_colorTop.rgb, _colorBottom.rgb, IN.alt);
	color.rgb = lerp(_colorBottom.rgb, color.rgb, IN.fog);	// Note that other things call saturate(IN.fog), but I find it does nothing?
#endif
}


void DadFog(inout appdata_full v, inout Input o)
{
	float4 vertPos = mul(unity_ObjectToWorld, v.vertex);
	float3 eyePos = _WorldSpaceCameraPos;
	float distFromEye = length(UnityObjectToViewPos(v.vertex).xyz);

	float ySpan = _yTop - _yBottom;

	// 0.0 = high and visible, 1.0 = low and fogged
	float vertHazeRaw = (_yTop - vertPos.y) / ySpan;
	float eyeHazeRaw = (_yTop - eyePos.y) / ySpan;

	float vertHaze = clamp(vertHazeRaw, 0, 1);
	float viewDist = _viewMax - vertHaze * (_viewMax - _viewMin);
	float vertDist = min(viewDist, distFromEye) / viewDist;

	// Fog of 1.0 means you can see it perfectly.
	o.fog = 1 - vertDist * vertDist;
	o.alt = max(vertHazeRaw, eyeHazeRaw);
}

void DadFinalColor(Input IN, inout fixed4 color)
{
#ifdef UNITY_PASS_FORWARDADD
	UNITY_APPLY_FOG_COLOR(IN.fog, color, float4(0, 0, 0, 0));
#else
	float3 target = IN.alt < 0 ? lerp(_colorTop.rgb, _colorAbove.rgb, -IN.alt) : lerp(_colorTop.rgb, _colorBottom.rgb, IN.alt);
	color.rgb = lerp(target.rgb, color.rgb, IN.fog);	// Note that other things call saturate(IN.fog), but I find it does nothing?
#endif
}
