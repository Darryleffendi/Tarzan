using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_FollowState : AnimalState, IAnimalCheckTrigger
{
    private float navTimer = 0;

    public Pet_FollowState(Animal animal, Player player) : base(animal)
    {
        this.player = player;
        if (player == null) this.player = Player.Instance;
        animal.animator.SetBool("isAttacking", false);
        animal.animator.SetBool("isWalking", true);
        animal.isPet = true;

        if(animal.GetWeapon().layer == LayerMask.NameToLayer("Enemy"))
        {
            animal.GetWeapon().layer = LayerMask.NameToLayer("Player");
            animal.SwitchUI();
            this.player.AddPets();
        }
    }

    public override void Next()
    {
        // Find TD Enemies
        animal.animator.SetBool("isWalking", true);
        animal.animator.SetBool("isRunning", true);
        animal.SetState(new Pet_TowerState(animal, player));
    }

    public void Next(Animal target)
    {
        // Attack wild animals
        animal.animator.SetBool("isWalking", true);
        animal.animator.SetBool("isRunning", true);
        animal.SetState(new Pet_AttackState(animal, player, target));
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

    public override void Update()
    {

        if (Time.time - navTimer < 0)
            return;
        navTimer = Time.time + 0.5f;

        animal.animator.SetBool("isAttacking", false);

        if (TowerDefense.Instance.GetGameStatus())
            Next();

        Collider[] colliders = Physics.OverlapSphere(animal.transform.position, 7, -1);

        foreach (Collider hit in colliders)
        {
            Animal target = hit.GetComponent<Animal>();

            if (target != null && target != animal && !target.isPet)
            {
                Next(target);
                return;
            }
        }

        if (Vector3.Distance(player.transform.position, animal.transform.position) <= 2.5f)
        {
            animal.StopAgent(true);
            animal.animator.SetBool("isWalking", false);
            animal.animator.SetBool("isRunning", false);
        }
        else
        {
            animal.StopAgent(false);
            animal.animator.SetBool("isWalking", true);
            animal.animator.SetBool("isRunning", true);
            animal.SetDestination(player.transform.position);
        }
    }

    public override void TakeDamage(int damage)
    {
        
    }
}
