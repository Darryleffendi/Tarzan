using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimalState : ITakesDamage
{
    protected Player player;
    protected Animal animal;
    protected bool isActive;
    public AnimalState(Animal animal)
    {
        this.animal = animal;
        isActive = true;
    }

    public abstract void Next();
    public abstract void Update();

    public virtual void TakeDamage(int damage)
    {
        animal.SetState(new Animal_HurtState(animal, player, this, damage));
    }
}
