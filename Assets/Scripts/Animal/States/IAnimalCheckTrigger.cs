using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimalCheckTrigger
{
    public void OnTriggerEnter(Collider other);
    public void OnTriggerStay(Collider other);
    public void OnTriggerExit(Collider other);
}
