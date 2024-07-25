using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_AttackState : AnimalState, IAnimalCheckTrigger
{
    private Animal target;
    private float navTimer;

    public Pet_AttackState(Animal animal, Player player, Animal target) : base(animal)
    {
        this.target = target;
        this.player = player;
    }

    public override void Next()
    {
        animal.animator.SetBool("isAttacking", false);
        animal.SetState(new Pet_FollowState(animal, player));
    }

    public override void Update()
    {
        if (Time.time - navTimer < 0)
            return;
        navTimer = Time.time + 0.5f;

        if (target.isPet) Next();

        float distance = Vector3.Distance(animal.transform.position, target.transform.position);
        animal.SetDestination(target.transform.position);

        if (distance < 3f)
            animal.animator.SetBool("isAttacking", true);
        else if (distance >= 3f)
            animal.animator.SetBool("isAttacking", false);
        else if (distance > 7)
            Next();
        else if (Vector3.Distance(player.transform.position, animal.transform.position) > 7)
            Next();
    }

    public void OnTriggerEnter(Collider other)
    {
    }

    public void OnTriggerStay(Collider other)
    {

    }

    public void OnTriggerExit(Collider other)
    {

    }

    public Animal GetTarget()
    {
        return target;
    }
}
