using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingDrone : Drone {

    public GameObject wallDetector;
    public GameObject edgeDetector;
    public bool walk = true;
	private bool reacting = false;
	private bool turning = false;
    // Use this for initialization
	new void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	new void Update () {
        base.Update();
	}

    new void FixedUpdate ()
    {
        base.FixedUpdate();
		if (walk) {
			rb.velocity = new Vector2(2 * transform.localScale.x, 0);
        } else {
            rb.velocity = new Vector2(0, 0);
        }
		//reacting = false;
    }

    public void HitEdge()
    {
        StopCoroutine("TrunAround");
        StartCoroutine("TurnAround");
    }

	public void ReactToBark(Vector3 point){
		if (!reacting) {
			reacting = true;
			bool tmp = Vector3.Angle (transform.InverseTransformPoint (point), transform.position - transform.InverseTransformPoint (point)) > 90;
			walk = false;
			if (!turning) {
				if (tmp) {
					print ("turn");
					transform.localScale = new Vector3 (
						transform.localScale.x * -1,
						transform.localScale.y,
						transform.localScale.z);
				}
				walk = true;
			}
			StartCoroutine ("Reacting");
		}
	}

	IEnumerator Reacting(){
		if (turning) {
			yield return new WaitForSeconds (6.5f);
		} else {
			yield return new WaitForSeconds (5.0f);
		}
		reacting = false;
		walk = true;
	}

    IEnumerator TurnAround()
    {
		while (reacting) {
			walk = false;
			yield return null;
		}
		turning = true;
		walk = false;
		yield return new WaitForSeconds (2.0f);
		transform.localScale = new Vector3 (
			transform.localScale.x * -1,
			transform.localScale.y,
			transform.localScale.z);
		
		walk = true;
		turning = false;
		
    }
}
