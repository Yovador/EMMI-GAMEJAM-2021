using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private List<Sprite> spriteList = new List<Sprite>();
    [SerializeField] private VictoryMenuController victoryMenu;
    [SerializeField] private PauseMenuController pauseMenu;
    [SerializeField] private LostMenuController lostMenu;


    public void DisappearHand()
    {
        rightHand.SetActive(false);
    }

    public void AppearHand()
    {
        rightHand.SetActive(true);
    }

    public IEnumerator LeftAttackAnimation(float timeBetweenFrame)
    {
        foreach (Sprite sprite in spriteList)
        {
            leftHand.GetComponent<Image>().sprite = sprite;
            yield return new WaitForSecondsRealtime(timeBetweenFrame);
        }

    }

    public void ResetLeftHand()
    {
        leftHand.GetComponent<Image>().sprite = spriteList[0];
    }

    public void Victory()
    {
        Cursor.lockState = CursorLockMode.None;
        victoryMenu.StartVictory();
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.startPause();
    }

    public void Lost()
    {
        Cursor.lockState = CursorLockMode.None;
        lostMenu.startLost();
    }
}
