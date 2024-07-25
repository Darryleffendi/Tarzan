using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject victoryScreen, loseScreen, canvas;
    [SerializeField]
    private CinemachineFreeLook mainCam;
    [SerializeField]
    private CinemachineVirtualCamera transitionCam;

    void Start()
    {
        if (SettingsMenu.Instance.gameStatus == GameStatus.Victory)
        {
            victoryScreen.SetActive(true);
            loseScreen.SetActive(false);
        }
        else if (SettingsMenu.Instance.gameStatus == GameStatus.Lose)
        {
            loseScreen.SetActive(true);
            victoryScreen.SetActive(false);
        }
        AudioManager.Instance.Background("EndBGM");
    }

    public void MainMenu()
    {
        AudioManager.Instance.SoundEffect("ButtonPress");
        canvas.SetActive(false);
        mainCam.m_Priority = 0;
        transitionCam.m_Priority = 10;
        Invoke(nameof(Navigate), 1.9f);
    }

    private void Navigate()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
