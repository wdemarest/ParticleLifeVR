using UnityEngine;

public class GameTime : Reliable
{
	const float pausedTimeScale = 0.0001f;
	const float regularTimeScale = 1.0f;
	[ReadOnly] public float originalFixedDeltaTime;

	float timer = 0;
	float frames = 0;
	[ReadOnly] public float FPS = 0;
	[ReadOnly] public float fixedDeltaTimeReadout;

	public bool isPaused { get { return Time.timeScale <= pausedTimeScale; } }

	public override void AwakeAlways()
	{
		originalFixedDeltaTime = Time.fixedDeltaTime;
		Debug.Assert(originalFixedDeltaTime > 0);
	}
	public void PauseToggle()
	{
		Time.timeScale = !isPaused ? pausedTimeScale : regularTimeScale;
		Time.fixedDeltaTime = originalFixedDeltaTime * Time.timeScale;
	}
	void Update()
	{
		fixedDeltaTimeReadout = Time.fixedDeltaTime;

		if (timer > 0)
		{
			timer -= Time.deltaTime;
			frames++;
		}
		if (timer <= 0)
		{
			timer = 1.0f;
			FPS = frames;
			frames = 0;
		}
	}


}
