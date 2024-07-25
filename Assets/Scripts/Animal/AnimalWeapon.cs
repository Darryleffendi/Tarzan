using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalWeapon : MonoBehaviour, IWeapon
{
    [SerializeField]
    private Animal animal;
   
    public int GetDamage()
    {
        return animal.GetDamage();
    }

    public Animal GetAnimal()
    {
        return animal;        
    }
}
