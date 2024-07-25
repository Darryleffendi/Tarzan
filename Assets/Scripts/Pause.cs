using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    private bool isPaused;

    public static Pause Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) OpenPause();
            else ClosePause();
        }
    }

    public void OpenPause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        UIController.Instance.ShowPlayerCanvas(false);
        UIController.Instance.ShowCursor(true);
        Time.timeScale = 0f;
    }

    public void ClosePause()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        AudioManager.Instance.SoundEffect("ButtonPress");
        UIController.Instance.ShowPlayerCanvas(true);
        UIController.Instance.ShowCursor(false);
        Time.timeScale = 1f;
    }

    public bool GetPausedStatus()
    {
        return isPaused;
    }

    public void Settings()
    {
        AudioManager.Instance.SoundEffect("ButtonPress");
        SettingsMenu.Instance.OpenSettings();
    }

    public void Menu()
    {
        ClosePause();
        AudioManager.Instance.SoundEffect("ButtonPress");
        UIController.Instance.ShowCursor(true);
        SceneChanger.Instance.MenuScene();
    }
}
