using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Layer
{
	public static int Head = -1;
	public static int HeadMask = -1;
	public static int BigHead = -1;
	public static int BigHeadMask = -1;
	public static int Hand = -1;
	public static int HandMask = -1;
	public static int Held = -1;
	public static int HeldMask = -1;
	public static int GrabbableTerrain = -1;
	public static int GrabbableTerrainMask = -1;
	public static int Fruit = -1;
	public static int FruitMask = -1;
	public static int Harvester = -1;
	public static int HarvesterMask = -1;
	public static int Item = -1;
	public static int ItemMask = -1;
	public static int ConvexHull = -1;
	public static int ConvexHullMask = -1;
	public static int WithLeaves = -1;
	public static int WithLeavesMask = -1;
	public static int AcidBubble = -1;
	public static int AcidBubbleMask = -1;
	public static int Zone = -1;
	public static int ZoneMask = -1;
	public static int Treasure = -1;
	public static int TreasureMask = -1;
	public static int MenuOption = -1;
	public static int MenuOptionMask = -1;
	public static int Shield = -1;
	public static int ShieldMask = -1;

	public static void StaticInit()
	{
		Head = LayerMask.NameToLayer("Head");
		HeadMask = 1 << Head;
		BigHead = LayerMask.NameToLayer("BigHead");
		BigHeadMask = 1 << Head;
		Hand = LayerMask.NameToLayer("Hand");
		HandMask = 1 << Hand;
		Held = LayerMask.NameToLayer("Held");
		HeldMask = 1 << Held;
		GrabbableTerrain = LayerMask.NameToLayer("GrabbableTerrain");
		GrabbableTerrainMask = 1 << GrabbableTerrain;
		Fruit = LayerMask.NameToLayer("Fruit");
		FruitMask = 1 << Fruit;
		Harvester = LayerMask.NameToLayer("Harvester");
		HarvesterMask = 1 << Harvester;
		Item = LayerMask.NameToLayer("Item");
		ItemMask = 1 << Item;
		ConvexHull = LayerMask.NameToLayer("ConvexHull");
		ConvexHullMask = 1 << ConvexHull;
		WithLeaves = LayerMask.NameToLayer("WithLeaves");
		WithLeavesMask = 1 << WithLeaves;
		AcidBubble = LayerMask.NameToLayer("AcidBubble");
		AcidBubbleMask = 1 << AcidBubble;
		Zone = LayerMask.NameToLayer("Zone");
		ZoneMask = 1 << Zone;
		Treasure = LayerMask.NameToLayer("Treasure");
		TreasureMask = 1 << Treasure;
		MenuOption = LayerMask.NameToLayer("MenuOption");
		MenuOptionMask = 1 << MenuOption;
		Shield = LayerMask.NameToLayer("Shield");
		ShieldMask = 1 << Shield;
	}
}
