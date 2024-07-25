using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_TowerState : AnimalState
{
    private float navTimer = 0;
    private TDEnemyManager tde;
    private Lumberjack target;

    public Pet_TowerState(Animal animal, Player player) : base(animal)
    {
        this.player = player;
        if (player == null) this.player = Player.Instance;
        animal.animator.SetBool("isAttacking", false);
        animal.animator.SetBool("isWalking", true);
        tde = TowerDefense.Instance.GetEnemyManager();
        if (tde.GetEnemies().Count <= 0) Next();
        target = tde.GetEnemies()[Random.Range(0, tde.GetEnemies().Count)];
    }

    public override void Next()
    {
        // Back to follow
        animal.SetState(new Pet_FollowState(animal, player));
    }

    public override void Update()
    {
        if (Time.time - navTimer < 0)
            return;
        navTimer = Time.time + 0.5f;

        if (!TowerDefense.Instance.GetGameStatus())
            Next();

        if (tde.GetEnemies().Count <= 0)
            Next();
        else if (target == null)
            target = tde.GetEnemies()[Random.Range(0, tde.GetEnemies().Count)];

        if (Vector3.Distance(animal.transform.position, target.transform.position) < 3f)
        {
            animal.animator.SetBool("isAttacking", true);
        }
        else
            animal.animator.SetBool("isAttacking", false);

        animal.SetDestination(target.transform.position);
    }

    public override void TakeDamage(int damage)
    {

    }

    public Lumberjack GetTarget()
    {
        return target;
    }
}
