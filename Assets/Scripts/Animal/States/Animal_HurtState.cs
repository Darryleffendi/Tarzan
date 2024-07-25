using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal_HurtState : AnimalState
{
    private AnimalState prevState;
    private float timeDelay;
    private bool dead;

    public Animal_HurtState(Animal animal, Player player, AnimalState prevState, int damage) : base(animal)
    {

        this.player = player;
        this.prevState = prevState;
        animal.StopAgent(true);
        animal.DeductHealth(damage);
        

        timeDelay = Time.time + 0.3f;
    }

    public override void Update()
    {
        if (animal.GetHealth() <= 0)
        {
            dead = true;
            animal.StopAgent(false);
            animal.Death(player);
        }

        if (!dead && Time.time - timeDelay >= 0)
        {
            animal.StopAgent(false);
            animal.SetState(prevState);
        }
    }

    public override void Next()
    {
        throw new System.Exception("Invalid Next Argument on Animal_HurtState.cs\n");
    }
}
