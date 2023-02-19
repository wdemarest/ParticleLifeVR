using UnityEngine;


public class Reliable : MonoBehaviour
{
	public virtual string canonicalName { get { return ""; } }

	public string path { get { return gameObject.GetNamePath() + "." + GetType().Name; } }

	public bool isPrefab { get { return gameObject.IsPrefab(); } }

	public void Assert(bool value)
	{
		if (!value)
			Debug.LogError("Failed assert " + gameObject.GetNamePath());
	}
	public virtual void AwakeAlways()
	{
	}
	public virtual void OnGameStart()
	{
	}
	public virtual void OnEditStart()
	{
	}
	public virtual void OnEditorChange()
	{
	}
	void Awake()
	{
		// THIS MUST BE HERE because objects instantiated during runtime need this call.
		// It is also true that things instantiated but set inactive will get NO AwakeAlways() call.
		AwakeAlways();
	}
}
