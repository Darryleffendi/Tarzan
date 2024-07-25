using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFactory
{
    Object Create(Vector3 spawnpoint);
}
