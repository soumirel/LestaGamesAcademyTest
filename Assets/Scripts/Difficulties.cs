using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulties : MonoBehaviour
{
    public void Easy()
    {
        PlayerPrefs.SetInt("difficulty", 100);
        SceneManager.LoadScene("MainGame");
    }

    public void Normal()
    {
        PlayerPrefs.SetInt("difficulty", 800);
        SceneManager.LoadScene("MainGame");
    }

    public void Hard()
    {
        PlayerPrefs.SetInt("difficulty", 1500);
        SceneManager.LoadScene("MainGame");
    }
}
