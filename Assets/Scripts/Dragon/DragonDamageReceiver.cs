using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonDamageReceiver : MonoBehaviour, ITakesDamage
{
    [SerializeField]
    private Dragon dragon;

    public void TakeDamage(int damage)
    {
        dragon.TakeDamage(damage);
    }
}
