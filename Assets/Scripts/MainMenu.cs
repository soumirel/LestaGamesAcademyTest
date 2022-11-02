using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _hideMenuClip;


    private IEnumerator PlayEnumerator()
    {
        _audioSource.PlayOneShot(_hideMenuClip);
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene("MainGame");
    }

    public void Play()
    {
        StartCoroutine(PlayEnumerator());
    }

    private IEnumerator CreditsEnumerator()
    {
        _audioSource.PlayOneShot(_hideMenuClip);
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene("Credits");
    }

    public void Credits()
    {
        StartCoroutine(CreditsEnumerator());
    }


    public void Exit()
    {
        _audioSource.PlayOneShot(_hideMenuClip);
        Application.Quit();
    }
}
