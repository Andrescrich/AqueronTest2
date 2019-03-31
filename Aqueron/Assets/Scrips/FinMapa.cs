using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinMapa : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player")) {
            Invoke("FinMapaRestart", 1f);
        }
    }

    void FinMapaRestart()
    {
        FindObjectOfType<GameManager>().Restart();
    }
}
