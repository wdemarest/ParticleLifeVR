using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanInput
{
	KeyboardControl keyboard { get { return HumanUser.instance.keyboard; } }
	MouseControl mouse { get { return HumanUser.instance.mouse; } }
	bool isMouseMode { get { return HumanUser.instance.isMouseMode; } }
	bool hasControllers { get { return HumanUser.instance.hasControllers; } }
	public void MouseInput(Grab grab, Movement movement)
	{
		if (isMouseMode != (Cursor.lockState == CursorLockMode.Locked))
		{
			Cursor.lockState = isMouseMode ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = !hasControllers && !isMouseMode;
			//Debug.Log("Cursor locked=" + (Cursor.lockState == CursorLockMode.Locked));
		}

		bool wasMouseMode = isMouseMode;
		HumanUser.instance.isMouseMode = hasControllers ? false : Input.GetKeyDown(KeyCode.Tab) ? !isMouseMode : isMouseMode;
		if (wasMouseMode && !isMouseMode && grab.isActive)
		{
			grab.Cancel();
		}

		if (isMouseMode && !grab.isActive)
		{
			Transform cam = movement.centerEyeAnchor.transform;
			mouse.Turn(Vector3.up, cam);
		}
		if (!hasControllers)
		{
			mouse.PositionHands(movement.ovrCameraRig);
			Vector3 move = keyboard.GetMovement(movement.centerEyeAnchor.transform);
			if (move != Vector3.zero)
			{
				movement.rb.velocity = Vector3.zero; // Since we're moving it directly, we don't want physics competing with us.
			}
			movement.positionChanger += move;
		}
	}
	public void KeyboardInput()
	{
	}
}
