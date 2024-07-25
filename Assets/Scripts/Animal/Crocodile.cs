using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crocodile : Animal
{
    public override void AttackSound()
    {
        audio.Animal("CrocodileAttack" + Random.Range(1, 4));
    }

    public override void Death(Player player)
    {
        SetState(new Pet_FollowState(this, player));
        Player.Instance.AddXP(initialHealth);
    }

    public override void StartChild()
    {
        initialHealth = 150;
        health = initialHealth;
        speed = 0.8f;
        runSpeed = 3.5f;
        damage = 15;
        state = new Animal_IdleState(this);
    }
}
