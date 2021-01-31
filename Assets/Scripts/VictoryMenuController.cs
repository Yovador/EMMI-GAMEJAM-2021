using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenuController : MonoBehaviour
{
    [SerializeField] private string MainMenu;
    [SerializeField] private string Level;
    private float timeScaleBeforePause;


    public void RetryButton()
    {
        restartTime();
        SceneManager.LoadScene(Level);
    }

    public void MainMenuButton()
    {
        restartTime();
        gameObject.SetActive(false);
        SceneManager.LoadScene(MainMenu);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    private void restartTime()
    {
        Time.timeScale = timeScaleBeforePause;
    }

    public void StartVictory()
    {
        gameObject.SetActive(true);
        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0;
    }
}
