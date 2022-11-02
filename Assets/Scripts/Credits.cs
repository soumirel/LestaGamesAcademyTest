using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _unshowMenuClip;


    private IEnumerator ToMenuEnumerator()
    {
        _audioSource.PlayOneShot(_unshowMenuClip);
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene("Menu");
    }

    public void ToMenu()
    {
        StartCoroutine(ToMenuEnumerator());
    }
}
