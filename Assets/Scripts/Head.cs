using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Head : Reliable
{
	public void _SetPosition(Vector3 position) { base.transform.position = position; }

	public Transform _transform { get { return base.transform; } }
	new public bool transform { get { return false; } }
	public Vector3 position { get { return _transform.position; } }
	public Quaternion rotation { get { return _transform.rotation; } }
	public Vector3 forward { get { return _transform.forward; } }
	public Vector3 eulerAngles { get { return _transform.eulerAngles; } }

	public TouchStack touchStack;

	void BeginTouching(GameObject go)
	{
	}
	void HaltAllTouching(GameObject go)
	{
	}
	public void OnObjectDestroy(GameObject aboutToChange)
	{
	}
	public override void AwakeAlways()
	{
		touchStack = new TouchStack();
		touchStack.onAddFn = BeginTouching;
		touchStack.onRemoveFn = HaltAllTouching;
	}
	void Update()
	{
		touchStack.Prune();
	}
	//
	// Remember that these messages all come from proxies.
	//
	public void OnTriggerEnter(Collider collider)
	{
		GameObject target = collider.gameObject.Resolve();
		if (target == null) return;
		Debug.Log("head hit " + target.GetNamePath());
		touchStack.Add(target);
	}

	//
	// Remember that these messages all come from proxies.
	//
	public void OnTriggerExit(Collider collider)
	{
		GameObject target = collider.gameObject.Resolve();
		if (target == null) return;
		if (touchStack.Contains(target))
		{
			//Debug.Log("head OUT " + target.GetNamePath());
			touchStack.Remove(target);
		}
	}
	public void OnBigHeadTriggerEnter(Collider collider)
	{
		OnTriggerEnter(collider);
	}
	public void OnBigHeadTriggerExit(Collider collider)
	{
		OnTriggerExit(collider);
	}
}
