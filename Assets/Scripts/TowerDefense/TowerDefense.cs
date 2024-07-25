using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefense : MonoBehaviour
{
    private int wave;
    private int health;
    private TDEnemyManager tdEnemies;
    [SerializeField]
    private Transform crystal, healthCanvas, healthBar;
    private bool gameStarted;

    public static TowerDefense Instance { get; private set; }
    void Awake()
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

    private void Start()
    {
        tdEnemies = GetComponent<TDEnemyManager>();
        health = 10;
        wave = 0;
    }

    private void Update()
    {

    }

    public void StartGame()
    {
        wave++;
        for(int i = 0; i < wave; i ++)
        {
            tdEnemies.Spawn();
        }
        UpdateHealth();
        healthCanvas.gameObject.SetActive(true);
        tdEnemies.Initialize();
        gameStarted = true;
    }

    public void StopGame()
    {
        healthCanvas.gameObject.SetActive(false);
        AudioManager.Instance.SwitchBGM("MainBGM");
        gameStarted = false;
    }

    public int GetWave()
    {
        return wave;
    }

    public Transform GetCrystal()
    {
        return crystal;
    }

    public bool GetGameStatus()
    {
        return gameStarted;
    }

    public void UpdateHealth()
    {
        healthBar.localScale = new Vector3(health / 10.0f, 1f, 1f);
    }

    public void DecrementHealth()
    {
        health--;
        if (health <= 0)
        {
            UIController.Instance.ShowCursor(true);
            SettingsMenu.Instance.gameStatus = GameStatus.Lose;
            SceneChanger.Instance.EndScene();
        }
        UpdateHealth();
    }

    public TDEnemyManager GetEnemyManager()
    {
        return tdEnemies;
    }
}
