using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    bool ended = false;
    public float restartDelay = 1.5f;
    public GameObject NivelMontañasUI;
    public GameObject NivelAlcantarillaUI;
    public GameObject ComienzoAlcantarillaUI;
    public GameObject ComienzoMontañaUI;
    public GameObject deathUI;
    public GameObject Life1;
    public GameObject Life2;
    public GameObject Life3;


    public void CompleteLevel()
    {
        NivelMontañasUI.SetActive(true);
        Invoke("Montaña", 1f);
    }

    public void NivelAlcantarilla()
    {
        NivelAlcantarillaUI.SetActive(true);
        Invoke("Alcantarilla", 1f);
    }

    public void ComienzoMontaña()
    {
        ComienzoMontañaUI.SetActive(true);
    }

    public void ComienzoAlcantarilla()
    {
        ComienzoAlcantarillaUI.SetActive(true);
    }

    public void Death()
    {
        FindObjectOfType<AudioManager>().Play("Salta");
    }

    public void EndGame ()
    {
        if (!ended)
        {
            ended = true;
            Debug.Log("GAME OVER");
            Invoke("Restart", restartDelay);
            deathUI.SetActive(true);
        }
    }

    public void Restart ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Health3 () {
        Life3.SetActive(false);
    }

    public void Health2()
    {
        Life2.SetActive(false);
    }

    public void Health1()
    {
        Life1.SetActive(false);
    }

    void Alcantarilla()
    {
        SceneManager.LoadScene(2);
    }

    void Montaña()
    {
        SceneManager.LoadScene(1);
    }
}
