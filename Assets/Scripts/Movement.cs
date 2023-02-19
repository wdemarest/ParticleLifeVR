using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Reliable
{
	public Player player;
	public Head head;

	public Rigidbody rb;
	public Grab grab;
	public Jump jump;
	public OVRCameraRig ovrCameraRig;
	public GameObject centerEyeAnchor;
	public SphereCollider sphereCollider;

	Vector3 lastCenterEyePosition;

	public float headRadius { get { return sphereCollider.radius; } }

	public Hand grabHand { get { return !grab.isActive ? null : (grab.IsGrabbingWith(player.handLeft.handId) ? player.handLeft : player.handRight); } }

	public float gravity = 0.5f;

	public float jumpSpeedScalar = 1.5f;        // This is applied to all your jumps, to give you more power
	public float jumpSpeedBase = 4.0f;        // Jump speed at start of game
	[ReadOnly] public float jumpSpeedCurrent = 0;
	public float jumpSpeedLimit
	{
		get
		{
			jumpSpeedCurrent = jumpSpeedBase;
			return jumpSpeedCurrent;
		}
	}

	public float totalSpeedLimit = 13f;

	public Vector3 positionChanger
	{
		get { return rb.position; }
		set { rb.position = value; }
	}

	public override void AwakeAlways()
	{
		Physics.gravity = new Vector3(0, -gravity, 0);
		Physics.autoSimulation = false;

		rb.useGravity = true;
		rb.isKinematic = false;
		rb.detectCollisions = true;
		rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

		Application.onBeforeRender += OnBeforeRenderCallback;

		ovrCameraRig._skipUpdate = false;
		ovrCameraRig.KenUpdateAnchors(true, true);
		ovrCameraRig.transform.position = rb.position - (centerEyeAnchor.transform.position - ovrCameraRig.transform.position);
		lastCenterEyePosition = centerEyeAnchor.transform.localPosition;
	}
	public void ApplyJump(Vector3 jumpVelocity)
	{
		float jumpSpeed = jumpVelocity.magnitude;
		float jumpScalar = jumpSpeedScalar;
		float jumpLimit = jumpSpeedLimit;

		jumpVelocity = jumpVelocity.normalized * Mathf.Min(jumpSpeed * jumpScalar, jumpLimit);
		rb.velocity = jumpVelocity;
	}
	public void ApplyJump()
	{
		Vector3 velocityUponJump = jump.GetVelocity();
		ApplyJump(velocityUponJump);
		jump.ClearSampleHistory();
	}
	public void SetNormal()
	{
		rb.velocity = Vector3.zero;
		rb.useGravity = DetermineGravity();
		rb.detectCollisions = true;
	}
	public void SetForGrab(GameObject steed)
	{
		rb.velocity = steed?.GetComponent<Rigidbody>() == null ? Vector3.zero : steed.GetComponent<Rigidbody>().velocity;
		rb.useGravity = false;
		rb.detectCollisions = true;
		jump.ClearSampleHistory();
	}
	public bool DetermineGravity()
	{
		return true;
	}
	public void UpdateEverything()
	{
		Vector3 GetGrabHandPosition()
		{
			return !grab.isActive ? Vector3.zero : (grab.IsGrabbingWith(player.handLeft.handId) ? ovrCameraRig.leftHandAnchor.position : ovrCameraRig.rightHandAnchor.position);
		}
		if (!grab.isActive)
		{
			bool grav = DetermineGravity();
			if (grav != rb.useGravity)
				rb.useGravity = grav;
		}
		if (!Physics.autoSimulation)
			Physics.Simulate(Time.deltaTime);

		if (rb.velocity.magnitude > totalSpeedLimit)
		{
			//Debug.Log("speed limit");
			rb.velocity *= totalSpeedLimit / rb.velocity.magnitude;
		}

		if (grab.isActive)
		{
			Assert(rb.velocity == Vector3.zero);
		}

		bool errorCollide = Physics.CheckSphere(rb.position, headRadius, grab.collideLayerMask);
		if (errorCollide)
		{
			Debug.Log("error collide");
			RaycastHit h;
			bool headHit = Physics.SphereCast(rb.position, headRadius, new Vector3(1,0,0), out h, 0.01f, grab.collideLayerMask);
			if (headHit)
				Debug.Log(h.collider.gameObject.GetNamePath());
		}

		Vector3 lastGrabHandPosition = GetGrabHandPosition();

		ovrCameraRig._skipUpdate = false;
		ovrCameraRig.KenUpdateAnchors(true, true);


		grab.ReportHand(player.handLeft.handId, ovrCameraRig.leftHandAnchor.position);
		grab.ReportHand(player.handRight.handId, ovrCameraRig.rightHandAnchor.position);

		Vector3 eyeDelta = centerEyeAnchor.transform.localPosition - lastCenterEyePosition;
		lastCenterEyePosition = centerEyeAnchor.transform.localPosition;

		Vector3 delta = eyeDelta;

		if (grab.isActive)
		{
			Vector3 handDelta = lastGrabHandPosition - GetGrabHandPosition();
			Vector3 steedDelta = grab.GetSteedDelta();
			jump.AddSample((handDelta + steedDelta) / Time.deltaTime);

			delta += handDelta;
			delta += steedDelta;
		}

		float distance = delta.magnitude;
		RaycastHit hitInfo;
		bool headHitWall = Physics.SphereCast(rb.position, headRadius, delta.normalized, out hitInfo, distance, grab.collideLayerMask);

		if (headHitWall)
		{
			Debug.Log("Hit wall");
			float hitDistance = hitInfo.distance; // - 0.00001f;    // Add a little tolerance to make sure we aren't hitting.
			float distInside = distance - hitDistance;
			if (grab.isActive)
			{
				grab.grabOffset += distInside * delta.normalized;
				Debug.Log("grabOffset = " + grab.grabOffset.magnitude);
				player.handLeft.controller.VibrateOneFrame();
				player.handRight.controller.VibrateOneFrame();
			}

			delta = hitDistance * delta.normalized;
		}

		//Debug.Log("RB moving " + delta.ToString("0.000000"));
		rb.position = rb.position + delta;
		ovrCameraRig.transform.position = rb.position - (centerEyeAnchor.transform.position - ovrCameraRig.transform.position);

		//head._SetPosition(centerEyeAnchor.transform.position);
		//player._SetPosition(centerEyeAnchor.transform.position);
		//Debug.Log((centerEyeAnchor.transform.position.y - _yLast).ToString("0.000000"));

		if (grab.isActive && grab.OffsetTooBig())
		{
			grab.ForceRelease(grabHand);
		}
		if (grab.isActive && !grabHand.uxTriggerDown)
		{
			grab.ReleaseAndJump(grabHand);
		}
	}
	void OnBeforeRenderCallback()
	{
		if (ovrCameraRig == null)
			return;
//		ovrCameraRig._skipUpdate = false;
//		ovrCameraRig.KenUpdateAnchors(true, false);
//		ovrCameraRig.transform.position = rb.position - (centerEyeAnchor.transform.position - ovrCameraRig.transform.position);
		UpdateEverything();
	}
	void LateUpdate()
	{
//		UpdateEverything();
	}
	void FixedUpdate()
	{
		//UpdateEverything();
	}
}
