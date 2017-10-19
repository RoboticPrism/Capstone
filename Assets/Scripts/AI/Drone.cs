using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour {

    public int suspicion = 0;
    public int maxSuspicion = 120;
    public bool seesPlayer = false;
    public bool alerted = false;
    protected Rigidbody2D rb;

    // Use this for initialization
    protected void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	protected void Update () {
		if (suspicion >= maxSuspicion)
        {
            alerted = true;
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
}
