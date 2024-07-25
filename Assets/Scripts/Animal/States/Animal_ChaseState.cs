using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal_ChaseState : AnimalState
{
    private float timer = 0f;

    public Animal_ChaseState(Animal animal, Player player) : base(animal)
    {
        animal.animator.SetBool("isRunning", true);
        this.player = player;
    }

    public override void Update()
    {
        if (!isActive) return;

        if (timer - Time.time > 0f)
            return;

        animal.RunSpeed();
        timer = Time.time + 0.5f;
        animal.SetDestination(player.transform.position);

        if (Vector3.Distance(animal.transform.position, player.transform.position) > 20f)
            Next();
        if (Vector3.Distance(animal.transform.position, player.transform.position) < 3.5f)
            Next(player);
    }

    public override void Next()
    {
        isActive = false;
        animal.animator.SetBool("isRunning", false);
        animal.SetState(new Animal_IdleState(animal, player));
    }

    public void Next(Player player)
    {
        isActive = false;
        animal.animator.SetBool("isRunning", false);
        animal.SetState(new Animal_AttackState(animal, player));
    }
}
