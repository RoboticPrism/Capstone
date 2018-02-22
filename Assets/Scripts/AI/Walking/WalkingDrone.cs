using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingDrone : Drone {

    public GameObject wallDetector;
    public GameObject edgeDetector;
    public bool walk = true;
	private bool reacting = false;
	private bool turning = false;
	private bool grounded = true;
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
		RaycastHit2D middle = Physics2D.Raycast (new Vector3(transform.position.x, transform.position.y - this.GetComponent<BoxCollider2D> ().bounds.extents.y - 0.01f, transform.position.z), Vector2.down, 0.1f);
		grounded = (middle && middle.collider);
		if (walk && grounded) {
			rb.velocity = new Vector2(2 * transform.localScale.x, this.rb.velocity.y);
        } else {
			rb.velocity = new Vector2(0, this.rb.velocity.y);
        }
		//reacting = false;
    }

    public void HitEdge()
    {
		if (this.gameObject.activeInHierarchy) {
			StopCoroutine ("TrunAround");
			StartCoroutine ("TurnAround");
		}
    }

	public void ReactToBark(Vector3 point){
		if (!reacting) {
			reacting = true;
			bool tmp = Vector3.Angle (transform.InverseTransformPoint (point), transform.position - transform.InverseTransformPoint (point)) > 90;
			walk = false;
			if (!turning) {
				if (tmp) {
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
		
		while (reacting || !grounded) {
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
