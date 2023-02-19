using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Controller
{
	string name;
	InputDeviceCharacteristics characteristics;
	InputDevice _device;
	bool toldFail = false;

	float vibDurationRemaining = 0f;

	public Controller(string name, InputDeviceCharacteristics characteristics)
	{
		this.name = name;
		this.characteristics = characteristics;
	}
	public bool isValid { get { return device.isValid; } }
	public bool isLeft { get { return name == "left"; } }
	public bool isRight { get { return name == "right"; } }
	public InputDevice device
	{
		get
		{
			if (!_device.isValid)
			{
				var inputDeviceList = new List<InputDevice>();
				InputDevices.GetDevicesWithCharacteristics(characteristics, inputDeviceList);
				foreach (var inputDevice in inputDeviceList)
				{
					Debug.Log("Found " + name);
					_device = inputDevice;
				}
			}
			if (!_device.isValid && !toldFail)
			{
				Debug.Log("Failed to find " + name);
				toldFail = true;
			}
			return _device;
		}
	}
	private void NativeVibrate(float time, float strength)
	{
		OVRInput.SetControllerVibration(time, strength, (name == "left" ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch));
	}

	public void Vibrate(float magnitude, float duration)
	{
		NativeVibrate(1f, magnitude);
		vibDurationRemaining = Mathf.Max(vibDurationRemaining, duration);
	}

	public void VibrateOneFrame(float vibStrength = 0.5f)
	{
		const float vibOneFrame = 1f / 40f; // 0.01f;
		Vibrate(vibStrength, vibOneFrame);
	}

	public void UpdateVibration()
	{
		if (vibDurationRemaining <= 0f)
		{
			NativeVibrate(0, 0);
			vibDurationRemaining = 0f;
		}
		if (vibDurationRemaining > 0f)
		{
			vibDurationRemaining -= Time.deltaTime;
		}
	}
}
