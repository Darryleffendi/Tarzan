using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal_IdleState : AnimalState
{
    Vector3 prevPos = Vector3.zero;
    float randTimer = 0;

    public Animal_IdleState(Animal animal) : base(animal) {

    }

    public Animal_IdleState(Animal animal, Player player) : base(animal)
    {
        this.player = player;
    }

    public override void Update()
    {
        if (!isActive) return;

        if(randTimer - Time.time < 0)
        {
            randTimer = Time.time + 10;
            Vector3 point = (animal).GetNest().RandomPoint(true);
            animal.SetDestination(point);
            animal.WalkSpeed();

            if (Vector3.Distance(prevPos, animal.transform.position) > 1)
            {
                animal.animator.SetBool("isWalking", true);
            }
            else
            {
                animal.animator.SetBool("isWalking", false);
            }
        }

        if(player != null)
        {
            if (Vector3.Distance(animal.transform.position, player.transform.position) < 20f)
            {
                Next(player);
            }
        }
    }

    public override void Next()
    {
        throw new System.Exception("Invalid Next Argument on Animal_IdleState.cs\nProvide player target.");
    }

    public void Next(Player player)
    {
        isActive = false;
        (animal).SetState(new Animal_ChaseState(animal, player));
    }

    public void SetTarget(Player player)
    {
        this.player = player;
    }

    public void RemoveTarget()
    {
        player = null;
    }
}
