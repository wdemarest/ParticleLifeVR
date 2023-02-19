using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class Util
{
	public static string GetNamePath(this GameObject go)
	{
		string s = "";
		while (go != null)
		{
			s = "/" + go.name + s;
			go = go.transform.parent?.gameObject;
		}
		return s;
	}
	public static bool IsPrefab(this GameObject gameObject)
	{
		return gameObject.scene.name == null;
	}
	public static GameObject Resolve(this GameObject target)
	{
		while (target.CompareTag("Child"))
		{
			target = target.transform.parent.gameObject;
		}
		return target;
	}
	public static void TraverseMine(this GameObject gameObject, Action<GameObject> fn)
	{
		// I'm pretty sure that referencing child.gameObject will properly access the actual game object even when inactive.
		foreach (Transform child in gameObject.transform)
		{
			fn(child.gameObject);
		}
	}
	public static void TraverseTree(this GameObject gameObject, Action<GameObject> fn)
	{
		// I'm pretty sure that referencing child.gameObject will properly access the actual game object even when inactive.
		foreach (Transform child in gameObject.transform)
		{
			fn(child.gameObject);
			child.gameObject.TraverseTree(fn);
		}
	}
	public static void TraverseAll(this GameObject gameObject, Action<GameObject> fn)
	{
		fn(gameObject);
		gameObject.TraverseTree(fn);
	}
	public static void TraverseComponentsMine<T>(this GameObject gameObject, Action<T> fn) where T : Component
	{
		Component[] list = gameObject.GetComponents(typeof(T));
		foreach (Component c in list)
		{
			fn((T)c);
		}
	}
	public static void TraverseComponentsAll<T>(this GameObject source, bool getInactive, Action<T> fn) where T : Component
	{
		// WARNING: The call GetComponentsInChildren is recursive.
		Component[] raw = source.GetComponentsInChildren(typeof(T), getInactive);
		foreach (var item in raw)
		{
			fn(item as T);
		}
	}

}