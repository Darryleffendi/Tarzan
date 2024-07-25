using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField]
    private Player player;

    public int GetDamage()
    {
        return player.GetDamage();
    }
}
