using UnityEngine;

public class CambiarCancionTheme3 : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Player")
        {
            FindObjectOfType<AudioManager>().Stop("Theme");
            FindObjectOfType<AudioManager>().Play("Theme3");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Player")
        {
            FindObjectOfType<AudioManager>().Stop("Theme3");
            FindObjectOfType<AudioManager>().Play("Theme");
        }
    }
}
