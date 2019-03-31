using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartelMontaña : MonoBehaviour {

    public GameObject cartelMontaña;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        cartelMontaña.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            cartelMontaña.SetActive(false);
    }
}
