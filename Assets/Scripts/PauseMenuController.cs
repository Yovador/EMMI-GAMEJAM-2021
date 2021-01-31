using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private string mainMenu;
    [SerializeField] private GameObject pauseMenu;
    private float timeScaleBeforePause;
    public void ContinueButton()
    {
        restartTime();
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
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

    public void startPause()
    {
        pauseMenu.SetActive(true);
        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0;
    }
}
