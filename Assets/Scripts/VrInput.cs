using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class VrInput
{
	public Controller controller { get { return controllerFn(); } }

	public bool isLeft { get { return controller.isLeft; } }
	public bool isRight { get { return controller.isRight; } }

	public bool menuButtonDown;
	public bool primaryButtonDown;
	public bool secondaryButtonDown;
	public float triggerAxis;
	public bool triggerDown;
	public float gripAxis;
	public bool gripDown;
	public Vector2 thumbAxis;
	public bool thumbDown;

	public bool lastMenuButtonDown = false;
	public bool lastPrimaryButtonDown = false;
	public bool lastSecondaryButtonDown = false;
	public bool lastTriggerDown = false;
	public bool lastGripDown = false;
	public Vector2 lastThumbAxis = Vector2.zero;
	public bool lastThumbDown = false;

	public bool menuButton { get { return menuButtonDown && !lastMenuButtonDown; } }
	public bool primaryButton { get { return primaryButtonDown && !lastPrimaryButtonDown; } }
	public bool secondaryButton { get { return secondaryButtonDown && !lastSecondaryButtonDown; } }
	public bool trigger { get { return triggerDown && !lastTriggerDown; } }
	public bool grip { get { return gripDown && !lastGripDown; } }
	public bool thumb { get { return thumbDown && !lastThumbDown; } }

	public Func<Controller> controllerFn;
	public VrInput(Func<Controller> fn)
	{
		controllerFn = fn;
	}
	public void Clear()
	{
		menuButtonDown = false;
		primaryButtonDown = false;
		secondaryButtonDown = false;
		thumbAxis = Vector2.zero;
		triggerAxis = 0.0f;
		triggerDown = false;
		gripAxis = 0.0f;
		gripDown = false;
		thumbDown = false;
	}
	public void Remember()
	{
		lastMenuButtonDown = menuButtonDown;
		lastPrimaryButtonDown = primaryButtonDown;
		lastSecondaryButtonDown = secondaryButtonDown;
		lastTriggerDown = triggerDown;
		lastGripDown = gripDown;
		lastThumbAxis = thumbAxis;
		lastThumbDown = thumbDown;
	}

	public void Read()
	{
		Remember();
		Clear();

		InputDevice device = controller.device;
		if (!device.isValid)
		{
			return;
		}

		device.TryGetFeatureValue(CommonUsages.primary2DAxis, out thumbAxis);

		device.TryGetFeatureValue(CommonUsages.trigger, out float triggerAxis);
		triggerDown = triggerAxis > 0.75f;

		device.TryGetFeatureValue(CommonUsages.grip, out float gripAxis);
		gripDown = gripAxis > 0.75f;

		device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out thumbDown);

		device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonDown);
		device.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButtonDown);
		device.TryGetFeatureValue(CommonUsages.menuButton, out menuButtonDown);
	}
}
