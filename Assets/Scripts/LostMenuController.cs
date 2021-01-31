using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LostMenuController : MonoBehaviour
{
    [SerializeField] private string mainMenu;
    [SerializeField] private GameObject lostMenu;
    private float timeScaleBeforePause;

    public void RestartButton()
    {
        restartTime();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MainMenuButton()
    {
        restartTime();
        SceneManager.LoadScene(mainMenu);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
    private void restartTime()
    {
        Time.timeScale = timeScaleBeforePause;
    }

    public void startLost()
    {
        lostMenu.SetActive(true);
        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0;
    }
}
