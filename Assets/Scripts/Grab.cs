using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : Reliable
{
	public Movement movement;

	public bool isActive { get { return handId != ""; } }

	string handId = "";
	public int collideLayerMask = 0;
	public int grabbableLayerMask = 0;
	public GameObject steed = null;
	Vector3 grabPosition;
	public Vector3 handPosition;
	Vector3 lastSteedPosition;

	public Vector3 grabOffset;
	public string mustReleaseTriggerBeforeGrabbing = "";

	public Vector3 steedPosition { get { return steed == null ? Vector3.zero : steed.transform.position; } }


	public override void AwakeAlways()
	{
		const bool allowGrabHarvesters = true;
		collideLayerMask = Layer.GrabbableTerrainMask | (allowGrabHarvesters ? Layer.HarvesterMask : 0);
		grabbableLayerMask = Layer.GrabbableTerrainMask | (allowGrabHarvesters ? Layer.HarvesterMask : 0);
		Assert(grabbableLayerMask != 0);
	}
	public bool IsGrabbingWith(string handId)
	{
		return this.handId == handId;
	}
	public bool MayGrabWith(string handId)
	{
		return !IsGrabbingWith(handId) && handId != mustReleaseTriggerBeforeGrabbing;
	}
/*
	public bool IsGrabbing(GameObject go)
	{
		if (!isActive || go == null || steed == null)
			return false;
		GameObject a = Interactable.Resolve(steed);
		GameObject b = Interactable.Resolve(go);
		return GameObject.ReferenceEquals(a, b);
	}
*/
	public bool OffsetTooBig()
	{
		return grabOffset.magnitude > 0.5f;
	}
	public void ApplyForceToSteed(Vector3 force)
	{
		Rigidbody steedRb = steed?.GetComponent<Rigidbody>();
		if (steedRb != null)
		{
			steedRb.AddForceAtPosition(force, handPosition, ForceMode.Impulse);
		}
	}
	public void Reset()
	{
		mustReleaseTriggerBeforeGrabbing = "";
		handId = "";
		movement.SetNormal();
	}
	public bool GrabObject(Hand hand, Vector3 handPosition, GameObject steed, string grabSource)
	{
		if (handId == hand.handId)
		{
			Debug.Log("WARNING: Repeated Hold on " + handId);
			return false;
		}
		if (hand.handId == mustReleaseTriggerBeforeGrabbing)
		{
			Debug.Log("WARNING: Must repleaseRepeated Hold on " + handId);
			return false;
		}
		if (steed != null && ((1 << steed.layer) & grabbableLayerMask) == 0)
		{
			Debug.LogWarning("WARNING: Steed " + steed.name + " bad layer mask");
		}

		mustReleaseTriggerBeforeGrabbing = handId;
		Debug.Log("Grab "+(steed==null?"nothing":steed.name)+" from "+ grabSource+ " with " + hand.handId + " " + handPosition);
		this.handId = hand.handId;
		this.steed = steed;
		this.grabPosition = handPosition;
		this.handPosition = handPosition;
		this.lastSteedPosition = steed != null ? steed.transform.position : Vector3.zero;
		this.grabOffset = Vector3.zero;
		movement.SetForGrab(steed);

		return true;
	}
	public void Cancel()
	{
		this.mustReleaseTriggerBeforeGrabbing = this.handId;
		this.handId = "";
		this.grabOffset = Vector3.zero;
		movement.SetNormal();
	}
	public void ForceRelease(Hand hand)
	{
		Debug.Log("Force Release " + handId);

		this.mustReleaseTriggerBeforeGrabbing = this.handId;
		this.handId = "";
		this.grabOffset = Vector3.zero;
		this.steed = null;
		movement.SetNormal();
	}
	public void ForceReleaseIfGrabbing(Hand hand, GameObject go)
	{
		if (IsGrabbingWith(hand.handId) && steed == go)
		{
			ForceRelease(hand);
		}
	}
	public void ReleaseAndJump(Hand hand)
	{
		//Debug.Log("Release " + handId);

		this.handId = "";
		this.grabOffset = Vector3.zero;
		this.steed = null;
		movement.SetNormal();
		movement.ApplyJump();
	}

	public void ReportHand(string handId, Vector3 handPosition)
	{
		if (this.handId == handId)
		{
			this.handPosition = handPosition;
		}
	}
	public Vector3 GetSteedDelta()
	{
		Vector3 steedDelta = steedPosition - lastSteedPosition;
		lastSteedPosition = steedPosition;
		return steedDelta;
	}
}
