using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Animal
{
    public override void AttackSound()
    {
        audio.Animal("WolfAttack" + Random.Range(1, 4));
    }

    public override void Death(Player player)
    {
        SetState(new Pet_FollowState(this, player));
        Player.Instance.AddXP(initialHealth);
    }

    public override void StartChild()
    {
        initialHealth = 130;
        health = initialHealth;
        damage = 8;
        speed = 1f;
        runSpeed = 4.2f;
        state = new Animal_IdleState(this);
    }
}
