using UnityEngine;

[ExecuteAlways]
public class FogVars : Reliable
{
	public float yTop;
	public float yBottom;
	public float viewMax;
	public float viewMin;

	public Color colorAbove;
	public Color colorTop;
	public Color colorBottom;

	void Update()
	{
		Shader.SetGlobalFloat("_yTop", yTop);
		Shader.SetGlobalFloat("_yBottom", yBottom);
		Shader.SetGlobalFloat("_viewMin", viewMin);
		Shader.SetGlobalFloat("_viewMax", viewMax);
		Shader.SetGlobalColor("_colorAbove", colorAbove);
		Shader.SetGlobalColor("_colorTop", colorTop);
		Shader.SetGlobalColor("_colorBottom", colorBottom);
	}
}
