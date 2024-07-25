using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocodileNest : AnimalNest
{
    public override void Spawn()
    {
        animals.Add((Crocodile)GetComponent<CrocodileFactory>().Create(RandomPoint()));
    }
}
