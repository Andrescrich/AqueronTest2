using UnityEngine;

public class cambioCancion : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Player")
        {
            FindObjectOfType<AudioManager>().Stop("Theme");
            FindObjectOfType<AudioManager>().Play("Theme2");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Player")
        {
            FindObjectOfType<AudioManager>().Stop("Theme2");
            FindObjectOfType<AudioManager>().Play("Theme");
        }
    }
}
