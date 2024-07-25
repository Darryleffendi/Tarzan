using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Animal : MonoBehaviour
{
    [SerializeField]
    private AnimalNest nest;
    [SerializeField]
    private GameObject weapon;
    [SerializeField]
    protected Transform healthBar, healthBackground, indicator;
    protected AnimalState state;
    protected NavmeshMovement navmesh;
    protected float initialHealth, health, speed, runSpeed;
    protected int damage;
    public bool isPet;
    protected Audio3D audio;

    [HideInInspector]
    public Animator animator;

    private void Start()
    {
        navmesh = GetComponent<NavmeshMovement>();
        animator = GetComponent<Animator>();
        audio = GetComponent<Audio3D>();
        StartChild();
    }

    public abstract void StartChild();
    public abstract void AttackSound();

    public abstract void Death(Player player);

    private void Update()
    { 
        if(state != null) state.Update();
    }
    public virtual void UpdateHealth()
    {
        healthBar.localScale = new Vector3(health / initialHealth * 0.0021f, 0.0005f, 1f);
        if (health > initialHealth * 2 / 3)
        {
            healthBar.GetComponent<Image>().color = Color.green;
        }
        else if (health > initialHealth * 1 / 3)
        {
            healthBar.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            healthBar.GetComponent<Image>().color = Color.red;
        }
        healthBar.GetComponent<Image>().CrossFadeAlpha(0.35f, 0f, false); ;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerWeapon weapon = other.GetComponent<PlayerWeapon>();
        if (weapon != null) state.TakeDamage(weapon.GetDamage());

        Debug.Log(other.gameObject.name);
        AnimalWeapon animalWeapon = other.GetComponent<AnimalWeapon>();
        if (animalWeapon != null)
        {
            if(animalWeapon.GetAnimal().isPet) state.TakeDamage(animalWeapon.GetDamage());
        }

        if (state is IAnimalCheckTrigger trigger)
            trigger.OnTriggerEnter(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (state is IAnimalCheckTrigger trigger)
            trigger.OnTriggerStay(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (state is IAnimalCheckTrigger trigger)
            trigger.OnTriggerExit(other);
    }

    public void DeductHealth(float damage)
    {
        health -= damage;
        UpdateHealth();
    }

    public void SwitchUI()
    {
        healthBar.gameObject.SetActive(false);
        healthBackground.gameObject.SetActive(false);
        indicator.gameObject.SetActive(true);
    }

    public float GetHealth()
    {
        return health;
    }

    public AnimalNest GetNest()
    {
        return nest;
    }

    public void SetNest(AnimalNest nest)
    {
        this.nest = nest;
    }

    public AnimalState GetState()
    {
        return state;
    }

    public void SetState(AnimalState state)
    {
        this.state = state;
    }

    public void SetDestination(Vector3 target)
    {
        navmesh.MoveToPoint(target);
    }

    public void StopAgent(bool x)
    {
        navmesh.Stop(x);
    }

    public void ActivateAttack()
    {
        AttackSound();
        weapon.SetActive(true);

        if(state is Pet_AttackState)
        {
            Animal target = ((Pet_AttackState)state).GetTarget();
            if(target != null)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < 3f)
                    target.state.TakeDamage(damage);
            }
        }
        else if(state is Pet_TowerState)
        {
            Lumberjack target = ((Pet_TowerState)state).GetTarget();
            if (target != null)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < 3f)
                    target.TakeDamage(damage);
            }
        }
    }

    public void DeactivateAttack()
    {
        weapon.SetActive(false);
    }

    public int GetDamage()
    {
        return damage;
    }

    public GameObject GetWeapon()
    {
        return weapon;
    }

    public float RemainingDistance()
    {
        return navmesh.RemainingDistance();
    }

    public void WalkSpeed()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);
        navmesh.SetSpeed(speed);
    }

    public void RunSpeed()
    {
        animator.SetBool("isRunning", true);
        navmesh.SetSpeed(runSpeed);
    }
}
