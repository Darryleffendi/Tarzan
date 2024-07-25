using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject PlayerCanvas;

    public static UIController Instance { get; private set; }

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

    void Start()
    {
        ShowCursor(false);
        AudioManager.Instance.Background("ForestAmbience");
        AudioManager.Instance.Background("MainBGM", true);
        if(SettingsMenu.Instance != null)
            SettingsMenu.Instance.gameStatus = GameStatus.Void;
    }

    public void ShowPlayerCanvas(bool x)
    {
        PlayerCanvas.SetActive(x);
    }

    public void ShowCursor(bool x)
    {
        Cursor.visible = x;
        
        if(!x)
        {
            Cursor.lockState = CursorLockMode.Locked;
            return;
        }
        Cursor.lockState = CursorLockMode.None;
    }
}
