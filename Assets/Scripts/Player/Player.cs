using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    protected float initialHealth, health, xp;
    protected int level, damage, petCount;
    public static Player Instance { get; private set; }

    private PlayerMovement movement;

    [SerializeField]
    protected Transform healthBar, experienceBar;
    [SerializeField]
    protected TextMeshProUGUI petCountText;

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
        movement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        initialHealth = 120;
        damage = 10;
        health = initialHealth;
        UpdateHealth();
        UpdateExp();
    }

    public PlayerMovement GetMovement()
    {
        return movement;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<AnimalNest>() != null)
        {
            other.GetComponent<AnimalNest>().PlayerEnter(this);
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            IWeapon weapon = other.GetComponent<IWeapon>();
            if (weapon != null)
                deductHealth(weapon.GetDamage());
        }
        if(other.gameObject.name == "NPCZone")
        {
            NPCDialogue.Instance.Activate(true);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<AnimalNest>() != null)
        {
            other.GetComponent<AnimalNest>().PlayerLeave();
        }
        if (other.gameObject.name == "NPCZone")
        {
            NPCDialogue.Instance.Activate(false);
        }
    }

    public void deductHealth(float amount)
    {
        health -= amount - level;

        if(health <= 0)
        {
            UIController.Instance.ShowCursor(true);
            SettingsMenu.Instance.gameStatus = GameStatus.Lose;
            SceneChanger.Instance.EndScene();
        }

        UpdateHealth();
    }

    public void AddXP(float amount)
    {
        xp += amount;

        if(xp >= 800)
        {
            LevelUp();
        }
        UpdateExp();
    }

    private void LevelUp()
    {
        health = initialHealth;
        xp -= 800;
        level++;
    }

    public void UpdateHealth()
    {
        healthBar.localScale = new Vector3(health / initialHealth, 1f, 1f);
    }

    public void UpdateExp()
    {
        experienceBar.localScale = new Vector3(xp / 800, 1f, 1f);
    }

    public void AddPets()
    {
        petCount++;
        petCountText.text = petCount.ToString() + " Pets";
    }

    public int GetDamage()
    {
        return Mathf.FloorToInt(damage + level);
    }
}
