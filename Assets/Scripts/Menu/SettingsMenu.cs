using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public TMPro.TMP_Dropdown AADropdown;
    public TMPro.TMP_Dropdown graphicDropdown;
    public Transform settingsUI;
    private float volume = 1f;

    public GameStatus gameStatus;
    
    public static SettingsMenu Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        graphicDropdown.value = QualitySettings.GetQualityLevel();
        graphicDropdown.RefreshShownValue();
        AADropdown.value = QualitySettings.antiAliasing;
        AADropdown.RefreshShownValue();
    }

    public bool GetActive()
    {
        return settingsUI.gameObject.activeSelf;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void OpenSettings()
    {
        settingsUI.gameObject.SetActive(true);
    }

    public void CloseSettings()
    {
        AudioManager.Instance.SoundEffect("ButtonPress");
        settingsUI.gameObject.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        this.volume = volume;
        AudioManager.Instance.SetVolume(volume);
    }

    public float GetVolume()
    {
        return volume;
    }

    public void SetFullscreen(bool x)
    {
        Screen.fullScreen = x;
    }

    public void ChangeAntiAliasing(int i)
    {
        if(i != 0)
        {
            i = (int)Mathf.Pow(2, i);
        }

        QualitySettings.antiAliasing = i;
    }
}

public enum GameStatus
{
    Void = 0,
    Victory = 1,
    Lose = 2
}