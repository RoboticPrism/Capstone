using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingDrone : Drone
{

	public GameObject wallDetector;
	public GameObject edgeDetector;
	public bool walk = true;
	private bool reacting = false;
	private bool turning = false;
	private bool grounded = true;
	private float barkStart = 0.0f;
	private const float barkLength = 5.0f;
	// Use this for initialization
	new void Start ()
	{
		base.Start ();
	}
	
	// Update is called once per frame
	new void Update ()
	{
		base.Update ();
		if (!StateSaver.gameState.paused) {
			base.FixedUpdate ();
			RaycastHit2D middle = Physics2D.Raycast (new Vector3 (transform.position.x, transform.position.y - this.GetComponent<BoxCollider2D> ().bounds.extents.y - 0.01f, transform.position.z), Vector2.down, 0.1f);
			grounded = (middle && middle.collider);
			if (walk && grounded) {
				rb.velocity = velToAdd + new Vector2 (2 * transform.localScale.x, this.rb.velocity.y);
			}
			//reacting = false;
		} else {
			rb.velocity = velToAdd + Vector2.zero;
		}
	}

	public void HitEdge ()
	{
		if (this.gameObject.activeInHierarchy) {
			StopCoroutine ("TurnAround");
			StartCoroutine ("TurnAround");
		}
	}

	public void ReactToBark (Vector3 point)
	{
		if (!reacting) {
			StopCoroutine("TurnAround");
			reacting = true;
			barkStart = Time.time;
			print ("drone react");
			bool tmp = Vector3.Angle (transform.InverseTransformPoint (point), transform.position - transform.InverseTransformPoint (point)) > 90;
			//if (!turning) {
			if (tmp) {
				transform.localScale = new Vector3 (transform.localScale.x * -1,transform.localScale.y, transform.localScale.z);
			}
			//}
			StartCoroutine ("Reacting");
		}
	}

	IEnumerator Reacting ()
	{
		float curTime = Time.time;
		while (curTime - barkStart < barkLength) {
			yield return new WaitForSeconds(0.5f);
			curTime = Time.time;
		}
		reacting = false;
		walk = true;
	}

	IEnumerator TurnAround ()
	{
		
		while (reacting || !grounded) {
			walk = false;
			yield return null;
		}
		turning = true;
		walk = false;
		float totalWait = 0.0f;
		while (!reacting && totalWait >= 2.0f) {
			totalWait += 0.5f;
			yield return new WaitForSeconds (0.5f);
		}
		if(!reacting){
			transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		}
		walk = true;
		turning = false;
		
	}
}
