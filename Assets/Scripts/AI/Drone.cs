using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour {

    public int suspicion = 0;
    public int maxSuspicion = 120;
    public bool seesPlayer = false;
    public bool alerted = false;
    protected Rigidbody2D rb;
	protected Vector2 velToAdd = Vector2.zero;
	protected GameObject alertIcon;
    // Use this for initialization
    protected void Start () {
        rb = GetComponent<Rigidbody2D>();
		alertIcon = this.transform.Find ("AlertIcon").gameObject;
		alertIcon.SetActive (false);
	}
	
	// Update is called once per frame
	protected void Update () {
		RaycastHit2D left = Physics2D.Raycast (new Vector3(transform.position.x - this.GetComponent<BoxCollider2D> ().bounds.extents.x - 0.01f, transform.position.y - this.GetComponent<BoxCollider2D> ().bounds.extents.y - 0.01f, transform.position.z), Vector2.down, 0.1f);
		RaycastHit2D middle = Physics2D.Raycast (new Vector3(transform.position.x, transform.position.y - this.GetComponent<BoxCollider2D> ().bounds.extents.y - 0.01f, transform.position.z), Vector2.down, 0.1f);
		RaycastHit2D right = Physics2D.Raycast (new Vector3(transform.position.x + this.GetComponent<BoxCollider2D> ().bounds.extents.x + 0.01f, transform.position.y - this.GetComponent<BoxCollider2D> ().bounds.extents.y - 0.01f, transform.position.z), Vector2.down, 0.1f);

		velToAdd = Vector2.zero;
		if (left.rigidbody != null && left.rigidbody.isKinematic) {
			velToAdd = left.rigidbody.velocity;
		} else if (middle.rigidbody && middle.rigidbody.isKinematic) {
			velToAdd = middle.rigidbody.velocity;
		} else if (right.rigidbody && right.rigidbody.isKinematic) {
			velToAdd = right.rigidbody.velocity;
		}
		alerted = suspicion >= maxSuspicion;
		if (suspicion >= maxSuspicion)
        {
            SpawnHunter();
        }
		alertIcon.SetActive (alerted);

	}

    protected void FixedUpdate ()
    {
		if (seesPlayer && suspicion < maxSuspicion && !StateSaver.gameState.paused)
        {
            suspicion++;
        }
    }

    public void PlayerEnteredSight ()
    {
        seesPlayer = true;
        StopCoroutine("Cooldown");
    }

    public void PlayerLeftSight ()
    {
        seesPlayer = false;
        StartCoroutine("Cooldown");
    }

    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(3);
        suspicion = 0;
    }

    public void SpawnHunter()
    {
        GetComponentInParent<Room>().SpawnHunter();
    }


}
