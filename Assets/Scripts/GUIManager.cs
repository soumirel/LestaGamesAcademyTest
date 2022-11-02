using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _wonMenuGUI;

    [SerializeField]
    private GameObject _pauseButtonGUI;

    [SerializeField]
    private GameObject _pauseMenuGUI;

    [SerializeField]
    public List<GameObject> _chipRulesBorders;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _showMenuClip;

    [SerializeField]
    private AudioClip _hideMenuClip;

    public void Start()
    {
        GameManager.OnGameOver.AddListener(ShowWonMenu);
        GameManager.OnYellowComplete.AddListener(UpdateYellowRule);
        GameManager.OnOrangeComplete.AddListener(UpdateOrangeRule);
        GameManager.OnRedComplete.AddListener(UpdateRedRule);
    }


    public void ShowWonMenu()
    {
        _pauseButtonGUI.SetActive(false);
        _wonMenuGUI.SetActive(true);
    }


    private IEnumerator RestartGameEnumerator()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("MainGame");
    }


    public void RestartGame()
    {
        Time.timeScale = 1;
        _audioSource.PlayOneShot(_hideMenuClip);
        StartCoroutine(RestartGameEnumerator());
    }


    public void ShowPauseMenu()
    {
        _audioSource.PlayOneShot(_showMenuClip);
        Time.timeScale = 0;
        _pauseMenuGUI.SetActive(true);
        _pauseButtonGUI.SetActive(false);
    }


    public void ResumeGame()
    {
        _audioSource.PlayOneShot(_hideMenuClip);
        Time.timeScale = 1;
        _pauseMenuGUI.SetActive(false);
        _pauseButtonGUI.SetActive(true);
    }


    private IEnumerator ToMainMenuEnumerator()
    {
        Time.timeScale = 1;
        _audioSource.PlayOneShot(_hideMenuClip);
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("Menu");
    }


    public void ToMainMenu()
    {
        StartCoroutine(ToMainMenuEnumerator());
    }


    public void UpdateYellowRule(bool isCompleted)
    {
        _chipRulesBorders[0].GetComponent<Image>().enabled = isCompleted;
    }


    public void UpdateOrangeRule(bool isCompleted)
    {
        _chipRulesBorders[1].GetComponent<Image>().enabled = isCompleted;
    }


    public void UpdateRedRule(bool isCompleted)
    {
        _chipRulesBorders[2].GetComponent<Image>().enabled = isCompleted;
    }
}
