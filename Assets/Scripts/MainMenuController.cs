using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject ControlsMenu;
    [SerializeField] private GameObject CreditsMenu;
    [SerializeField] private string firstLevel;

    public void StartButton()
    {
        SceneManager.LoadScene(firstLevel);
    }

    public void ControlsButton()
    {
        MainMenu.SetActive(false);
        ControlsMenu.SetActive(true);
    }

    public void CreditsButton()
    {
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(true);
    }

    public void BackButton()
    {
        MainMenu.SetActive(true);
        ControlsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
    }
}
