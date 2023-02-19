using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

public static class Game
{
	public static GameReferences g;
	public static GameObject root;
	public static Player player;

	public static GameTime time { get { return g.time; } }
	public static bool isInitializing { get { return g.globalInit.initializing; } }
	public static bool isPaused { get { return Time.timeScale == 0.0f; } }
	public static bool isQuitting { get { return g.globalInit.quitting; } }

	public static void StaticInit()
	{
		float goalFPS = 72.0f;
		OVRPlugin.systemDisplayFrequency = goalFPS;

		root = GameObject.Find("Game");
		g = root.GetComponent<GameReferences>();
		player = g.player;
		Layer.StaticInit();
	}
}
