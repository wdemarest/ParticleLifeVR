using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : Reliable
{
	int velHistMax = 0;
	Vector3[] velHist;
	int velHistCount = 0;

	public override void AwakeAlways()
	{
		velHistMax = (int)(1 / Time.fixedDeltaTime);
		velHist = new Vector3[velHistMax];
	}
	public void ClearSampleHistory()
	{
		velHistCount = 0;
	}
	public Vector3 GetVelocity()
	{
		Debug.Assert(velHistMax != 0);

		//collects dotproducts of hand movement vectors

		//OHHHH SHIT DAWG! Higher dotproducts mean straighter angles between two vectors. This makes an array that start only once the dotproducts are above
		//the validThreshold (straight enough) and ends only once they're below the validThreshold.

		bool straightEnough = false;
		float validThreshold = 0.8f; //~35 degrees
		List<Vector3> validHist = new List<Vector3>();
		for (int i = 0; i < velHistCount - 1; i++)
		{
			float dotProduct = Vector3.Dot(velHist[i].normalized, velHist[i + 1].normalized);
			if (dotProduct > validThreshold) { straightEnough = true; }
			if (straightEnough)
			{
				validHist.Add(velHist[i]);
				if (dotProduct <= validThreshold) { break; }
			}
		}

		Vector3 AvgVel = new Vector3(0, 0, 0);

		if (validHist.Count == 0)
		{
			return AvgVel;
		}


		//Use angle of whole swing but only velocity of last few because we want the release velocity.
		for (int i = 0; i < validHist.Count; i++)
		{
			AvgVel += validHist[i] / validHist.Count;
		}

		Vector3 angle = AvgVel.normalized;


		float speed = 0.0f;
		int velSamplesTaken = Mathf.Min(validHist.Count, 3);
		for (int i = 0; i < velSamplesTaken; i++)
		{
			speed += validHist[i].magnitude / velSamplesTaken;
		}

		AvgVel = angle * speed;
		return AvgVel;
	}
	public void AddSample(Vector3 vel)
	{
		for (int i = velHistMax - 2; i >= 0; i -= 1)
		{
			velHist[i + 1] = velHist[i];
		}

		velHist[0] = vel;
		velHistCount = Mathf.Min(velHistCount + 1, velHistMax);
		//Debug.Log("velHist[0]="+ velHist[0]);
	}
}
