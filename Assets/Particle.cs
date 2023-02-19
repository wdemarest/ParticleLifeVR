using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
	public float guid;
	public int type;
	Rules rules;
	
	// Start is called before the first frame update
    void Start()
    {
		rules = GameObject.Find("Rules").GetComponent<Rules>();

		guid = Random.value;
		type = Random.Range(0, 2);
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		Collider[] foundColliders = Physics.OverlapSphere(transform.position, rules.rMax);
		foreach (var foundCollider in foundColliders)
		{
			GameObject particleFound = foundCollider.gameObject;
			if(particleFound.GetComponent<Particle>().guid == guid) continue;

			float dist = Vector3.Distance(transform.position, particleFound.transform.position);

			Vector3 move = particleFound.transform.position - transform.position;
			move.Normalize();

			if (dist > rules.rMin)
			{
				move *= rules.speed;
			}
			else
			{
				move *= -rules.rMinRepulsion;
			}

			move /= Mathf.Pow(dist, 2);
			move *= Time.deltaTime;

			GetComponent<Rigidbody>().AddForce(move.x, move.y, move.z);
		}
	}
}
