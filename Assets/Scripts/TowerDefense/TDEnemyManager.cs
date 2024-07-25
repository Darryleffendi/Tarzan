using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TDEnemyManager : MonoBehaviour
{
    private List<Lumberjack> lumberjackList;
    private LumberjackFactory lumberjackFactory;
    private TowerDefense towerDefense;
    private int enemyCount;
    private float totalHealth;
    private float health;

    [SerializeField]
    private Transform enemiesHealth, healthCanvas;
    [SerializeField]
    private TextMeshProUGUI countText;

    private void Start()
    {
        lumberjackList = new List<Lumberjack>();
        lumberjackFactory = GetComponent<LumberjackFactory>();
        towerDefense = GetComponent<TowerDefense>();
    }

    public void Spawn()
    {
        Lumberjack lumb = (Lumberjack)lumberjackFactory.Create();
        lumberjackList.Add(lumb);
    }

    public void TakeDamage(Lumberjack lumberjack, int damage)
    {
        if(lumberjack.GetHealth() <= 0)
        {
            Death(lumberjack);
        }
        health -= damage;
        UpdateHealth();
    }

    public void Death(Lumberjack lumberjack)
    {
        Player.Instance.AddXP(lumberjack.GetHealth());
        lumberjackList.Remove(lumberjack);
        Destroy(lumberjack.gameObject);
        if (lumberjackList.Count <= 0)
        {
            Invoke(nameof(Reset), 1f);
        }
    }

    public void Initialize()
    {
        foreach (Lumberjack lumb in lumberjackList)
        {
            totalHealth += lumb.GetHealth();
        }
        healthCanvas.gameObject.SetActive(true);
        health = totalHealth;
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        enemiesHealth.GetComponent<Image>().fillAmount = health / totalHealth;
        countText.text = lumberjackList.Count + " Enemies Left";
    }

    private void Reset()
    {
        totalHealth = 0;
        health = 0;
        healthCanvas.gameObject.SetActive(false);
        towerDefense.StopGame();
    }
    
    public List<Lumberjack> GetEnemies()
    {
        return lumberjackList;
    }
}
