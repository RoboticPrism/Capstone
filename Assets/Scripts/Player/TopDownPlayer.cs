using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownPlayer : MonoBehaviour {

    GameObject playerSprite;
    Rigidbody2D rb;
    public float speed = 1f;

	// Use this for initialization
	void Start () {
        playerSprite = transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        rb.velocity = new Vector2(
            Input.GetAxis("Horizontal") * speed * Time.deltaTime, 
            Input.GetAxis("Vertical") * speed * Time.deltaTime
        );
        if (rb.velocity.magnitude > .01f)
        {
            playerSprite.transform.rotation = Quaternion.RotateTowards(
                playerSprite.transform.rotation,
                Quaternion.AngleAxis(Mathf.Atan2(-rb.velocity.x, rb.velocity.y) * Mathf.Rad2Deg, Vector3.forward),
                10f
            );
        }
	}
}
