using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfNest : AnimalNest
{
    public override void Spawn()
    {
        animals.Add((Wolf)GetComponent<WolfFactory>().Create(RandomPoint()));
    }
}
