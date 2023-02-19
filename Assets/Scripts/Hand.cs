using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using TMPro;

public class Hand : Reliable
{
	public Player player;

	public Transform _transform { get { return base.transform; } }
	new public bool transform { get { return false; } }
	public Vector3 position { get { return _transform.position; } }
	public Quaternion rotation { get { return _transform.rotation; } }
	public Transform asParent { get { return _transform; } }

	[SerializeField] public string handId = "";

	public bool uxPrimaryButton = false;
	public bool uxSecondaryButton = false;
	public bool uxTriggerDown = false;
	public bool uxGripDown = false;
	public bool uxThumbDown = false;
	Vector2 lastThumbAxis = Vector2.zero;
	bool lastPrimaryButton = false;
	bool lastSecondaryButton = false;
	bool lastTriggerDown = false;
	bool lastGripDown = false;
	bool lastThumbDown = false;

	public TouchStack touchStack;
	//public GameObject meatHand;

	public Grab grab { get { return player.movement.grab; } }

	int solidCollisions { get { return touchStack.CountInLayer(grab.grabbableLayerMask); } }

	public bool isLeftHand { get { return handId == "left"; } }
	public bool isRightHand { get { return handId == "right"; } }

	public Controller controller { get { return HumanUser.instance.controller[handId]; } }

	public bool isGrabbing { get { return grab.IsGrabbingWith(handId); } }
	public bool ableToGrab { get { return grab.MayGrabWith(handId) && !uxGripDown; } }
	public Hand otherHand { get { return isLeftHand ? player.handRight : player.handLeft; } }

	public Touch touchCollider;

	public GameObject objectHandIsFullyWithin = null;

	public bool AllReleased()
	{
		return !uxTriggerDown && !uxGripDown && !uxPrimaryButton;
	}
	public bool IsTouching(GameObject gameObject)
	{
		return (objectHandIsFullyWithin == gameObject || touchStack.Contains(gameObject));
	}
	public void DetermineObjectHandIsFullyWithin(int layerMask)
	{
		Vector3 castDirection = touchCollider.transform.position - player.head.position;
		float length = castDirection.magnitude;

		bool isWithin = Physics.Raycast(
			player.head.position,
			castDirection.normalized,
			out RaycastHit hitInfo,
			length,
			layerMask
		);
		objectHandIsFullyWithin = isWithin ? hitInfo.transform.gameObject : null;
	}
	public override void AwakeAlways()
	{
		touchStack = new TouchStack();
		touchStack.onAddFn = BeginTouching;
		touchStack.onRemoveFn = HaltAllTouching;
	}
	void BeginTouching(GameObject go)
	{
	}
	public void HaltAllTouching(GameObject go)
	{
	}
	void FixedUpdate()
	{
	}
	void LateUpdate()
	{
		if (isGrabbing)
		{
			//meatHand.transform.localPosition = grab.grabOffset;
			//Debug.Log(grab.grabOffset);
		}
		else
		{
			//meatHand.transform.localPosition = Vector3.zero;
		}
	}
	void Update()
	{
		bool menuButton = false;
		Vector2 uxThumbAxis = Vector2.zero;
		uxTriggerDown = false;
		uxGripDown = false;
		uxPrimaryButton = false;
		uxSecondaryButton = false;

		void rememberLast()
		{
			lastPrimaryButton = uxPrimaryButton;
			lastSecondaryButton = uxSecondaryButton;
			lastTriggerDown = uxTriggerDown;
			lastGripDown = uxGripDown;
			lastThumbAxis = uxThumbAxis;
			lastThumbDown = uxThumbDown;
		}

		controller.UpdateVibration();

		InputDevice device = controller.device;
		if (device.isValid)
		{
			device.TryGetFeatureValue(CommonUsages.primary2DAxis, out uxThumbAxis);

			device.TryGetFeatureValue(CommonUsages.trigger, out float triggerAxis);
			uxTriggerDown = triggerAxis > 0.75f;

			device.TryGetFeatureValue(CommonUsages.grip, out float gripAxis);
			uxGripDown = gripAxis > 0.75f;

			device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out uxThumbDown);

			device.TryGetFeatureValue(CommonUsages.primaryButton, out uxPrimaryButton);
			device.TryGetFeatureValue(CommonUsages.secondaryButton, out uxSecondaryButton);
			device.TryGetFeatureValue(CommonUsages.menuButton, out menuButton);
		}

		// RESET GRAB HAND REGARDING THRUSTER USE
		if (!uxTriggerDown && grab.mustReleaseTriggerBeforeGrabbing == handId)
		{
			grab.mustReleaseTriggerBeforeGrabbing = "";
		}

		touchStack.Prune();

		//Debug.Log("solidCollisions=" + solidCollisions);
		// GRAB AND HOLD
		DetermineObjectHandIsFullyWithin(grab.grabbableLayerMask);

		// ATTEMPT TO GRAB A FULLY WITHIN HOLDABLE INTERACTABLE
		if (uxTriggerDown && ableToGrab)
		{
			// If I hit the trigger, and my hand was already colliding, OR
			// If my hand is completely within but not colliding with a mesh
			if (solidCollisions > 0 || objectHandIsFullyWithin != null)
			{
				if (solidCollisions > 0)
				{
					//Debug.Log("Grab due to solidCollisions " + solidCollisions);
					if (solidCollisions > 1)
						touchStack.Dump("Solid grab=" + solidCollisions + " ");
				}
				GameObject whatGrabbed = objectHandIsFullyWithin ? objectHandIsFullyWithin : touchStack.First;
				if (whatGrabbed == null || ((1 << whatGrabbed.layer) & grab.grabbableLayerMask) == 0)
				{
					whatGrabbed = touchStack.GetFirstLayerMatch(grab.grabbableLayerMask);
				}
				if (whatGrabbed != null)
				{
					grab.GrabObject(this, position, whatGrabbed, "hand within");
				}
			}
		}

		rememberLast();
	}
	public void OnObjectDestroy(GameObject aboutToChange)
	{
	}
	public void _OnTriggerEnter(Collider collider)
	{
		GameObject target = collider.gameObject.Resolve();
		if (target == null) return;
		//Debug.Log("HAND IN  " + target.GetNamePath());

		touchStack.Add(target);

		bool grabRequested = uxTriggerDown && ableToGrab;

		int myMask = (1 << target.layer);
		if (grabRequested && (myMask & grab.grabbableLayerMask) != 0)
		{
			//Debug.Log(handId + " touching " + target.name);
			grab.GrabObject(this, position, target, "OnTriggerEnter");
		}
	}

	public void _OnTriggerExit(Collider collider)
	{
		GameObject target = collider.gameObject.Resolve();
		if (target == null) return;
		//Debug.Log("HAND OUT " + target.GetNamePath());

		if (touchStack.Contains(target))
		{
			touchStack.Remove(target);
		}
	}



}