using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonWeapon : MonoBehaviour, IWeapon
{
    [SerializeField]
    private int damage;

    public int GetDamage()
    {
        return damage;
    }
}
