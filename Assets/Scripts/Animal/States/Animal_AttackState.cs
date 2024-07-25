using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal_AttackState : AnimalState
{

    public Animal_AttackState(Animal animal, Player player) : base(animal)
    {
        animal.animator.SetBool("isAttacking", true);
        this.player = player;
        animal.SetDestination(player.transform.position);
    }

    public override void Update()
    {
        if (Vector3.Distance(animal.transform.position, player.transform.position) > 3f)
            Next(player);
    }

    public override void Next()
    {
        animal.animator.SetBool("isAttacking", false);
        (animal).SetState(new Animal_IdleState(animal, player));
    }

    public void Next(Player player)
    {
        animal.animator.SetBool("isAttacking", false);
        (animal).SetState(new Animal_ChaseState(animal, player));
    }
}
