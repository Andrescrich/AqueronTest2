using UnityEngine;

public class NewLevel : MonoBehaviour
{

    public GameManager gameManager;
    public float delay = 1f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Player" && gameObject.tag == "Alcantarilla")
        {
            gameManager.ComienzoAlcantarilla();
        } 
        if (collision.GetComponent<Collider2D>().tag == "Player" && gameObject.tag == "Montaña")
        {
            gameManager.ComienzoMontaña();
        }
    }
}
