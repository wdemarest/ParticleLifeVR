using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HumanUser
{
	static HumanUser _instance = null;
	static public HumanUser instance { get { if (_instance == null) _instance = new HumanUser(); return _instance; } }

	public KeyboardControl keyboard = new KeyboardControl();
	public MouseControl mouse = new MouseControl();
	public Dictionary<string, Controller> controller = new Dictionary<string, Controller>();

	public bool hasControllers { get { return controller["head"].isValid && controller["left"].isValid && controller["right"].isValid; } }
	public Controller controllerLeft { get { return controller["left"]; } }
	public Controller controllerRight { get { return controller["right"]; } }

	// WARNING: This looks really stupid, and it is, but C# forbids internal references directly to members
	public VrInput vrInputLeft = new VrInput(() => { return instance.controller["left"]; });
	public VrInput vrInputRight = new VrInput(() => { return instance.controller["right"]; });

	public bool hasMouseKeyboard { get { return !hasControllers; } }
	[ReadOnly] public bool isMouseMode;

	public HumanUser()
	{
		controller["head"] = new Controller("head", InputDeviceCharacteristics.HeadMounted);
		controller["left"] = new Controller("left", InputDeviceCharacteristics.Left | InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller);
		controller["right"] = new Controller("right", InputDeviceCharacteristics.Right | InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller);
	}
}
