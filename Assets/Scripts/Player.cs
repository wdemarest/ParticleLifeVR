using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using TMPro;

public class Player : Reliable
{
	public void _SetPosition(Vector3 position) { base.transform.position = position; }

	new public bool transform { get { return false; } }
	public bool position { get { return false; } }
	public Vector3 up { get { return Vector3.up; } }

	public Movement movement;

	public Hand handLeft;
	public Hand handRight;
	public Head head;

	public Transform playerGroup;

	public HumanInput humanInput;

	void Update()
	{
		if (humanInput == null)
			humanInput = new HumanInput();
		humanInput.MouseInput(movement.grab, movement);
	}
}
