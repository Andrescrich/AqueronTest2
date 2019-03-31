using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelComplete : MonoBehaviour {

    public void LoadMontaña()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadAlcantarillas()
    {
        SceneManager.LoadScene(2);
    }

}
