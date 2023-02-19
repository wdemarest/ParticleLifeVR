using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchStack
{
	List<GameObject> list = new List<GameObject>();
	public Action<GameObject> onAddFn = null;
	public Action<GameObject> onRemoveFn = null;

	public int Count { get { return list.Count; } }
	public GameObject First { get { return list.Count > 0 ? list[0] : null; } }

	public void Traverse(Action<GameObject> actionFn)
	{
		int count = 0;
		while (count < list.Count)
		{
			if (list[count] == null)
			{
				list.RemoveAt(count);
			}
			else
			if (!list[count].activeInHierarchy)
			{
				onRemoveFn(list[count]);
				list.RemoveAt(count);
			}
			else
			{
				actionFn(list[count]);
				++count;
			}
		}
	}
	public GameObject GetFirstLayerMatch(int layerMask)
	{
		GameObject found = null;
		Traverse((GameObject go) =>
		{
			if (((1 << go.layer) & layerMask) != 0)
				found = go;
		});
		return found;
	}
	public int CountInLayer(int layerMask)
	{
		int found = 0;
		Traverse((GameObject go) =>
		{
			int objectLayerMask = 1 << go.layer;
			if ((objectLayerMask & layerMask) != 0)
				++found;
		});
		return found;
	}
	public void Clear()
	{
		// Do not use Iterate here because we're remove every single item.
		for (int i = 0; i < list.Count; ++i)
			if (list[i] != null)
				onRemoveFn(list[i]);
		list = new List<GameObject>();
	}
	public void Prune()
	{
		Traverse((GameObject go) => { });
	}
	public bool Contains(GameObject gameObject)
	{
		return list.Contains(gameObject);
	}
	public void Add(GameObject addMe)
	{
		if (addMe == null)
		{
			Debug.LogWarning("Error: Can not add null.");
			return;
		}
		if (!Contains(addMe))
		{
			list.Add(addMe);
			onAddFn(addMe);
		}
	}
	public bool Remove(GameObject removeMe)
	{
		if (removeMe == null)
		{
			Debug.LogWarning("Error: Can not remove null.");
			return false;
		}
		int index = list.FindIndex(go => go == removeMe);
		if (index != -1)
		{
			//Debug.LogWarning("touchStack removed " + removeMe.name);
			// Very important to remove the item BEFORE you call onRemoveFn, in case of callbacks.
			list.RemoveAt(index);
			onRemoveFn(removeMe);
			return true;
		}
		return false;
	}
	public void Dump(string msg)
	{
		string s = msg;
		Traverse((GameObject go) =>
		{
			s += "[" + go.name + " aih=" + (go.activeInHierarchy ? 1 : 0) + "]";
		});
		Debug.LogWarning(s);
	}
}

