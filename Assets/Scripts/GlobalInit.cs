using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GlobalInit : MonoBehaviour
{
	[ReadOnly] public HumanUser humanUser;

	[ReadOnly] public bool initializing;
	[ReadOnly] public bool quitting;
	[ReadOnly] public bool startWasAlreadyCalled = false;
	[ReadOnly] public float originalTimeScale = -1;

	void OnApplicationQuit()
	{
		quitting = true;
	}
	void RunAwakeAlways(bool onActives)
	{
		var reliableList = Resources.FindObjectsOfTypeAll<Reliable>();
		Debug.Log("Found " + reliableList.Length + " objects of type Reliable");

		int count = 0;
		foreach (var r in reliableList)
		{
			bool isPrefab = r.gameObject.scene.name == null;
			if (!isPrefab && (onActives || !r.gameObject.activeInHierarchy))
			{
				//Debug.Log("awakening " + r.path + "." + r.GetType().Name + ". " + count);
				r.AwakeAlways();
				//AnomalyTracker.Check("RunAwakeAlways after " + r.path);
			}
			count += 1;
		}
	}
	void Awake()
	{
		initializing = true;
		humanUser = new HumanUser();

		// Make sure that FixedUpdate is not called while we do this...
		originalTimeScale = Time.timeScale;
		Time.timeScale = 0;

		Game.StaticInit();
		if (Application.isPlaying)
		{
			RunAwakeAlways(false);
		}
	}
	void Start()
	{
		initializing = false;
		Time.timeScale = originalTimeScale;
	}
}
