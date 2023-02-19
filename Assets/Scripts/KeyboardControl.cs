using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControl
{
	float forwardMetersPerSecond = 2.5f;
	float rightMetersPerSecond = 2.5f;
	float runScalar = 10.0f;

	float forward = 0;
	float right = 0;

	public Vector3 GetMovement(Transform camera)
	{
		forward = 0;
		right = 0;
		if (Input.GetKey(KeyCode.W)) forward += 1;
		if (Input.GetKey(KeyCode.S)) forward += -1;
		if (Input.GetKey(KeyCode.A)) right += -1;
		if (Input.GetKey(KeyCode.D)) right += 1;
		if (Input.GetKey(KeyCode.LeftShift))
		{
			forward *= runScalar;
			right *= runScalar;
		}
		//Debug.Log("F:"+forward+", R="+right);

		return ((camera.forward * forward * forwardMetersPerSecond) + (camera.right * right * rightMetersPerSecond)) * Time.deltaTime;
	}
}
