using UnityEngine;

public class Horizon : Reliable
{
	public float lowestHorizionAllowed = 0.0f;
	public void CenterOnPlayer(Vector3 position)
	{
		transform.position = new Vector3(position.x, Mathf.Max(transform.position.y, lowestHorizionAllowed), position.z);
	}
	void LateUpdate()
	{
		CenterOnPlayer(Game.player.movement.centerEyeAnchor.transform.position);
	}
}
