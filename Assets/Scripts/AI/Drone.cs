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
    // Use this for initialization
    protected void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	protected void Update () {
		RaycastHit2D middle = Physics2D.Raycast (new Vector3(transform.position.x, transform.position.y - this.GetComponent<BoxCollider2D> ().bounds.extents.y - 0.01f, transform.position.z), Vector2.down, 0.1f);
		if (middle.rigidbody != null && middle.rigidbody.gameObject.GetComponent<SideScrollingPlayer>() == null) {
			velToAdd = middle.rigidbody.velocity;
		}
		if (suspicion >= maxSuspicion)
        {
            alerted = true;
            SpawnHunter();
        }
	}

    protected void FixedUpdate ()
    {
        if (seesPlayer && suspicion < maxSuspicion)
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
