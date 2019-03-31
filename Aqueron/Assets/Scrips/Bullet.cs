using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed = 20f;
    public Rigidbody2D rb;
    public GameObject AD;

	// Use this for initialization
	void Start () {
        Physics2D.IgnoreLayerCollision(11, 12);
        rb.velocity = transform.right * speed;
        FindObjectOfType<AudioManager>().Play("Disparo");
	}

    private void OnCollisionEnter2D(Collision2D hitInfo)
    {
        Destroy(gameObject);
        FindObjectOfType<AudioManager>().Play("Impacto");
        Instantiate(AD, transform.position, transform.rotation);
    }

}
