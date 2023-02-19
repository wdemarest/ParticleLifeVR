using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour
{
	public Hand hand;

	void OnTriggerEnter(Collider collider)
	{
		Debug.Log("touch " + hand.handId);
		hand._OnTriggerEnter(collider);
	}
	void OnTriggerExit(Collider collider)
	{
		hand._OnTriggerExit(collider);
	}
}
