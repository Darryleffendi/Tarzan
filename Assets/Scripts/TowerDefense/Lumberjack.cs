using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lumberjack : MonoBehaviour, ITakesDamage
{
    private int initialHealth;
    private int health;
    private float speed;

    private Animator animator;
    private TowerDefense td;
    
    private Vector3[] path;
    private Quaternion lookRotation;
    private int targetIndex;

    private bool moving = false;
    private bool pathFinish = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        lookRotation = transform.rotation;
        td = TowerDefense.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerWeapon weapon = other.GetComponent<PlayerWeapon>();
        if(weapon != null)
        {
            TakeDamage(weapon.GetDamage());
        }
    }

    public void SetAttributes(int health, float speed)
    {
        this.initialHealth = health;
        this.health = health;
        this.speed = speed;
    }

    public void SetPath(Vector3[] path)
    {
        this.path = path;
    }

    private void Update()
    {
        if(path != null && !moving && path.Length > 0)
        {
            moving = true;
            StartCoroutine(FollowPath());
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.1f);

        if(pathFinish)
        {
            if(Vector3.Distance(transform.position, td.GetCrystal().position) < 6.5)
            {
                if (animator.GetBool("isAttacking")) return;

                animator.SetBool("isAttacking", true);
                StartCoroutine(AttackTower());
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, td.GetCrystal().position, speed * Time.deltaTime);
            }
        }
    }

    private IEnumerator FollowPath()
    {
        Vector3 currentWaypoint;
        targetIndex = 0;

        animator.SetBool("isWalking", true);
        currentWaypoint = path[0];
        while (true)
        {
            if (Vector3.Distance(transform.position, currentWaypoint) <= 0.3f)
            {
                targetIndex++;

                if (targetIndex >= path.Length)
                {
                    pathFinish = true;
                    yield break;
                }

                currentWaypoint = path[targetIndex];
            }   

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            lookRotation = Quaternion.LookRotation(currentWaypoint - transform.position);
            yield return null;
        }
    }

    private IEnumerator AttackTower()
    {
        yield return new WaitForSeconds(1);
        td.DecrementHealth();
        td.GetEnemyManager().Death(this);
    }

    public int GetHealth()
    {
        return health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        td.GetEnemyManager().TakeDamage(this, damage);
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
