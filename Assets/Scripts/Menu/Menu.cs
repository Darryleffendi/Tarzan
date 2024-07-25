using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public Transform menuUI;

    void Start()
    {
        AudioManager.Instance.Background("MenuBGM");
        AudioManager.Instance.Background("ForestAmbience");
    }

    private void Update()
    {
        if(!SettingsMenu.Instance.GetActive())
        {
            menuUI.gameObject.SetActive(true);
        }
    }

    public void PlayGame()
    {
        AudioManager.Instance.SoundEffect("ButtonPress");
        SceneChanger.Instance.GameScene();
    }

    public void OpenSettings()
    {
        AudioManager.Instance.SoundEffect("ButtonPress");
        SettingsMenu.Instance.OpenSettings();
        menuUI.gameObject.SetActive(false);
    }

    public void ExitGame()
    {
        AudioManager.Instance.SoundEffect("ButtonPress");
        Application.Quit();
    }
}
