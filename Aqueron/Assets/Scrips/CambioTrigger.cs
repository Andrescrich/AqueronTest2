using UnityEngine;

public class CambioTrigger : MonoBehaviour
{

    public GameManager gameManager;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Player" && gameObject.tag == "Alcantarilla")
        {
            gameManager.NivelAlcantarilla();
        }
        if (collision.GetComponent<Collider2D>().tag == "Player" && gameObject.tag == "Montaña")
        {
            gameManager.CompleteLevel();
        }
    }
}