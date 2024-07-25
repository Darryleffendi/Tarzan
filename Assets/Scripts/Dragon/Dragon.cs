using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Dragon : Animal, ITakesDamage
{
    int specialAttackIndex = 2;
    Player player;
    NavMeshAgent agent;

    [SerializeField]
    Transform dragonNest;

    private float timeDelay;
    private bool isGrowling;

    public static Dragon Instance { get; private set; }

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
        agent = GetComponent<NavMeshAgent>();
    }

    public override void StartChild()
    {
        initialHealth = 600;
        health = initialHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerWeapon weapon = other.GetComponent<PlayerWeapon>();
        if (weapon != null)
        {
            TakeDamage(weapon.GetDamage());
        }
    }

    public override void UpdateHealth()
    {
        healthBar.GetComponent<Image>().fillAmount = health / initialHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            UIController.Instance.ShowCursor(true);
            SettingsMenu.Instance.gameStatus = GameStatus.Victory;
            SceneChanger.Instance.EndScene();
        }
        UpdateHealth();
    }

    public void SetPlayer(Player player)
    {
        if (player != null)
            healthBar.parent.gameObject.SetActive(true);
        else
            healthBar.parent.gameObject.SetActive(false);

        this.player = player;
    }

    public override void Death(Player player)
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        if (player == null || animator.GetBool("Attack2") || animator.GetBool("Attack3"))
            return;

        if (Mathf.FloorToInt(Time.time) % 10 == 0)
            SpecialAttack();
        if (Mathf.FloorToInt(Time.time) % 5 == 0 && !isGrowling)
        {
            audio.Dragon("Growl" + Random.Range(1, 4));
            isGrowling = true;
        }
        else isGrowling = false;

        if (Time.time - timeDelay > 0)
        {
            timeDelay = Time.time + 0.5f;
            if (Vector3.Distance(transform.position, player.transform.position) > 8)
            {
                animator.SetBool("isWalking", true);
                agent.SetDestination(player.transform.position);
            }
            else
            {
                animator.SetBool("Attack1", true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "CaveZone")
            agent.SetDestination(dragonNest.position);
    }

    private void SpecialAttack()
    {
        agent.ResetPath();
        animator.SetBool("Attack" + specialAttackIndex, true);
        specialAttackIndex++;
        if (specialAttackIndex > 3) specialAttackIndex = 2;
    }

    public override void AttackSound()
    {
        throw new System.NotImplementedException();
    }
}
