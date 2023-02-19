using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl
{
	public float turnSensitivity = 1.0f;
	public float moveSensitivity = 0.1f;
	public float moveScrollSensitivity = 10.0f;
	int ignoreCount = 10;

	float handPosDist = 0.5f;
	float handPosDown = 0.2f;

	public void Turn(Vector3 playerUp, Transform camera)
	{
		float mouseDx = Input.GetAxis("Mouse X");
		float mouseDy = -Input.GetAxis("Mouse Y");
		if (ignoreCount > 0)
		{
			--ignoreCount;
			return;
		}

		float deltaNod = mouseDy * 360.0f * turnSensitivity * Time.deltaTime;
		float deltaSpin = mouseDx * 360.0f * turnSensitivity * Time.deltaTime;

		camera.RotateAround(camera.position, playerUp, deltaSpin);
		camera.RotateAround(camera.position, camera.right, deltaNod);
	}

	public void Move(Vector3 up, Transform camera, Transform hand)
	{
		float mouseDx = Input.GetAxis("Mouse X");
		float mouseDy = Input.GetAxis("Mouse Y");

		float deltaX = mouseDx * 360.0f * moveSensitivity * Time.deltaTime;
		float deltaY = mouseDy * 360.0f * moveSensitivity * Time.deltaTime;
		float deltaZ = Input.mouseScrollDelta.y * moveScrollSensitivity * Time.deltaTime;

		hand.Translate(up * deltaY);
		hand.Translate(camera.right * deltaX);
		hand.Translate(camera.forward * deltaZ);
	}
	public void PositionHands(OVRCameraRig ovrCameraRig)
	{
		// Force the hands into positions that don't block the camera.
		Transform cam = ovrCameraRig.centerEyeAnchor.transform;
		ovrCameraRig.leftHandAnchor.position = cam.position + cam.forward * handPosDist + cam.up * -handPosDown + cam.right * -0.3f;
		ovrCameraRig.rightHandAnchor.position = cam.position + cam.forward * handPosDist + cam.up * -handPosDown;
		ovrCameraRig.leftHandAnchor.rotation = cam.rotation;
		ovrCameraRig.rightHandAnchor.rotation = cam.rotation;
	}
}
